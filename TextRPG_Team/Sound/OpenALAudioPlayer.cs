using OpenTK.Audio.OpenAL;

namespace TextRPG_Team.Sound;

class MultiThreadedOpenAlPlayer
{
    private readonly Queue<string> _audioFiles = new Queue<string>();
    private readonly Queue<int> _openAlBuffers = new Queue<int>();
    private readonly Lock _queueLock = new Lock();
    private readonly Lock _bufferLock = new Lock();
    private readonly Lock _playbackLock = new Lock();

    private bool _isRunning = true;
    private bool _isLooping; // 반복 재생 설정 플래그
    private Thread? _loaderThread;
    private Thread? _playerThread;

    private int _currentSource; // 현재 재생 중인 소스 ID
    private bool _isSwitching; // 음악 교체 플래그

    public void StartAudioProcessing()
    {
        // WAV 파일 로드 스레드 시작
        _loaderThread = new Thread(LoadAudioFiles) { IsBackground = true };
        _loaderThread.Start();

        // OpenAL 오디오 재생 스레드 시작
        _playerThread = new Thread(PlayAudio) { IsBackground = true };
        _playerThread.Start();
    }

    public void StopAudioProcessing()
    {
        _isRunning = false;

        // 스레드 종료 대기
        _loaderThread?.Join();
        _playerThread?.Join();
    }

    public void EnqueueAudioFile(string filePath, bool loop = false)
    {
        lock (_queueLock)
        {
            _audioFiles.Enqueue(filePath);
            _isLooping = loop; // 반복 재생 설정
        }
    }

    private void LoadAudioFiles()
    {
        while (_isRunning)
        {
            string? filePath = null;

            // 큐에서 파일 경로 가져오기
            lock (_queueLock)
            {
                if (_audioFiles.Count > 0)
                {
                    filePath = _audioFiles.Dequeue();
                }
            }

            if (filePath != null)
            {
                try
                {
                    var waveData = LoadWave(filePath, out var channels, out var bits, out var rate);

                    var buffer = AL.GenBuffer();
                    var format = GetSoundFormat(channels, bits);
                    AL.BufferData<byte>(buffer, format, waveData.AsSpan(), rate);
                    lock (_bufferLock)
                    {
                        _openAlBuffers.Enqueue(buffer);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[ERROR] WAV 파일 로드 중 오류: {ex.Message}");
                }
            }

            Thread.Sleep(10);
        }
    }

    void PlayAudio()
    {
        var source = AL.GenSource();

        lock (_playbackLock)
        {
            _currentSource = source;
        }

        while (_isRunning)
        {
            int buffer = 0;

            lock (_bufferLock)
            {
                if (_isSwitching)
                {
                    while (_openAlBuffers.Count > 0)
                    {
                        var oldBuffer = _openAlBuffers.Dequeue();
                        AL.DeleteBuffer(oldBuffer);
                    }

                    _isSwitching = false;
                }

                if (_openAlBuffers.Count > 0)
                {
                    buffer = _openAlBuffers.Dequeue();
                }
            }

            if (buffer != 0)
            {
                AL.Source(source, ALSourcei.Buffer, buffer);
                AL.SourcePlay(source);

                AL.GetSource(source, ALGetSourcei.SourceState, out var state);
                while ((ALSourceState)state == ALSourceState.Playing && !_isSwitching)
                {
                    Thread.Sleep(10);
                    AL.GetSource(source, ALGetSourcei.SourceState, out state);
                }

                if (_isSwitching)
                {
                    AL.SourceStop(source);
                }

                AL.DeleteBuffer(buffer);

                // 반복 재생 처리
                if (_isLooping && !_isSwitching)
                {
                    lock (_queueLock)
                    {
                        _audioFiles.Enqueue(buffer.ToString());
                    }
                }
            }

            Thread.Sleep(10);
        }

        // 소스 정리
        lock (_playbackLock)
        {
            AL.DeleteSource(source);
            _currentSource = 0;
        }
    }

    public static byte[] LoadWave(string filePath, out int channels, out int bits, out int rate)
    {
        using var reader = new BinaryReader(File.OpenRead(filePath));

        var chunkId = new string(reader.ReadChars(4));
        if (chunkId != "RIFF")
            throw new Exception("유효하지 않은 WAV 파일입니다.");

        reader.ReadInt32();
        var format = new string(reader.ReadChars(4));
        if (format != "WAVE")
            throw new Exception("유효하지 않은 WAV 파일입니다.");

        var subChunk1Id = new string(reader.ReadChars(4));
        if (subChunk1Id != "fmt ")
            throw new Exception("유효하지 않은 'fmt ' 청크입니다.");

        reader.ReadInt32();
        reader.ReadInt16();
        channels = reader.ReadInt16();
        rate = reader.ReadInt32();
        reader.ReadInt32();
        reader.ReadInt16();
        bits = reader.ReadInt16();

        while (true)
        {
            var subChunkId = new string(reader.ReadChars(4));
            var subChunkSize = reader.ReadInt32();

            if (subChunkId == "data")
            {
                return reader.ReadBytes(subChunkSize);
            }

            reader.BaseStream.Seek(subChunkSize, SeekOrigin.Current);
        }
    }

    public static ALFormat GetSoundFormat(int channels, int bits)
    {
        if (channels == 1 && bits == 8) return ALFormat.Mono8;
        if (channels == 1 && bits == 16) return ALFormat.Mono16;
        if (channels == 2 && bits == 8) return ALFormat.Stereo8;
        if (channels == 2 && bits == 16) return ALFormat.Stereo16;

        throw new NotSupportedException("지원하지 않는 오디오 포맷입니다.");
    }

    public void StopCurrentTrack()
    {
        lock (_playbackLock)
        {
            if (_currentSource != 0)
            {
                AL.SourceStop(_currentSource);
                AL.DeleteSource(_currentSource);
                _currentSource = 0;
            }
        }

        // 음악 정지 시 관련 플래그 초기화
        _isSwitching = true;
        _isLooping = false;
    }

    public bool IsAudioPlaying()
    {
        lock (_playbackLock)
        {
            // 현재 재생 소스가 없으면 false 반환
            if (_currentSource == 0)
                return false;

            // 소스 상태 확인
            AL.GetSource(_currentSource, ALGetSourcei.SourceState, out var state);
            if ((ALSourceState)state == ALSourceState.Playing)
                return true;

            // 상태가 Playing이 아닌 경우 false 반환
            return false;
        }
    }
}