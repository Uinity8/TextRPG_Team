using System.Collections.Concurrent;
using OpenTK.Audio.OpenAL;

namespace TextRPG_Team.Sound
{
    public static class SoundManager
    {
        private static readonly MultiThreadedOpenAlPlayer MusicPlayer = new(); // 배경 음악 플레이어
        private static readonly ConcurrentDictionary<string, int> EffectBuffers = new(); // 효과음 버퍼 저장
        private static readonly Lock VolumeLock = new(); // 볼륨 동시 접근 관리

        private static float _masterVolume = 1.0f; // 마스터 볼륨
        private static bool _isMuted; // 소리 끄기 여부
        private static string? _currentBackgroundMusic; // 현재 재생 중인 배경 음악 파일 경로

        // 초기화 메서드 (OpenAL 디바이스 설정)
        public static void Initialize()
        {
            string deviceName = ALC.GetString(ALDevice.Null, AlcGetString.DefaultDeviceSpecifier);
            var device = ALC.OpenDevice(deviceName);

            var context = ALC.CreateContext(device, (int[])null!);

            ALC.MakeContextCurrent(context);

            MusicPlayer.StartAudioProcessing(); // 배경 음악 플레이어 시작
        }

        private static CancellationTokenSource? _backgroundMusicTokenSource; // 반복 작업 제어용

        public static void PlayBackgroundMusic(string musicFilePath, bool loop = false)
        {
            if (_isMuted)
            {
                Console.WriteLine("[DEBUG] 소리가 꺼져있어 배경 음악을 재생하지 않습니다.");
                return;
            }

            if (!File.Exists(musicFilePath))
            {
                Console.WriteLine($"[ERROR] 배경 음악 파일이 존재하지 않습니다: {musicFilePath}");
                return;
            }

            // 기존 작업을 중지
            _backgroundMusicTokenSource?.Cancel();
            _backgroundMusicTokenSource = new CancellationTokenSource();
            var token = _backgroundMusicTokenSource.Token;

            // 기존 음악 중지 (이전 상태 정리)
            //StopBackgroundMusic();

            try
            {
                MusicPlayer.EnqueueAudioFile(musicFilePath); // 새로운 음악 추가
                _currentBackgroundMusic = musicFilePath; // 현재 재생 음악 기록
               // Console.WriteLine($"[DEBUG] 배경 음악 대기열에 추가되었습니다: {musicFilePath}");

                // 루프 처리
                if (loop)
                {
                    Task.Run(() =>
                    {
                        while (_currentBackgroundMusic == musicFilePath) // 중간에 음악이 변경되면 루프 종료
                        {
                            if (token.IsCancellationRequested) // 중지 요청 시 루프 종료
                                break;

                            if (!MusicPlayer.IsAudioPlaying()) // 음악이 재생 중이지 않으면
                            {
                                //Console.WriteLine("[DEBUG] 배경 음악 재시작: " + musicFilePath);
                                MusicPlayer.EnqueueAudioFile(musicFilePath); // 다시 큐에 추가
                            }

                            Task.Delay(500).Wait(); // 0.5초 간격으로 상태 확인
                        }
                    }, token);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] 배경 음악 재생 중 오류 발생: {ex.Message}");
            }
        }

        
        // 배경 음악 중지
        public static void StopBackgroundMusic()
        {
            if (!string.IsNullOrEmpty(_currentBackgroundMusic))
            {
                //Console.WriteLine($"[DEBUG] 배경 음악 중지: {_currentBackgroundMusic}");

                // 백그라운드 작업 중지
                _backgroundMusicTokenSource?.Cancel();
                _backgroundMusicTokenSource = null;

                // 현재 음악 정지
                MusicPlayer.StopCurrentTrack();
                _currentBackgroundMusic = null; // 현재 상태 초기화
            }
        }

        // 효과음 재생
        public static void PlayEffect(string effectFilePath)
        {
            if (_isMuted)
                return;

            if (string.IsNullOrWhiteSpace(effectFilePath) || !File.Exists(effectFilePath))
            {
                //Console.WriteLine($"[ERROR] 효과음 파일이 존재하지 않습니다: {effectFilePath}");
                return;
            }

            if (!EffectBuffers.TryGetValue(effectFilePath, out var buffer))
            {
                try
                {
                    // 효과음 버퍼 생성
                    var waveData = MultiThreadedOpenAlPlayer.LoadWave(effectFilePath, out var channels, out var bits,
                        out var rate);
                    buffer = AL.GenBuffer();
                    var format = MultiThreadedOpenAlPlayer.GetSoundFormat(channels, bits);
                    AL.BufferData<byte>(buffer, format, waveData.AsSpan(), rate);

                    EffectBuffers[effectFilePath] = buffer;
                }
                catch (Exception)
                {
                    //Console.WriteLine($"[ERROR] 효과음 파일 로드 실패: {ex.Message}");
                    return;
                }
            }

            // 효과음 소스 생성 및 재생
            var source = AL.GenSource();
            AL.Source(source, ALSourcei.Buffer, buffer);
            AL.SourcePlay(source);

            // 일정시간 후 소스 삭제
            Task.Delay(2000).ContinueWith(_ => AL.DeleteSource(source));
        }

        // 음소거 및 볼륨 설정을 통합
        public static void UpdateVolume()
        {
            lock (VolumeLock)
            {
                AL.Listener(ALListenerf.Gain, _isMuted ? 0.0f : _masterVolume);
            }
        }

        // 볼륨 설정 (0.0f - 1.0f)
        public static void SetVolume(float volume)
        {
            _masterVolume = Math.Clamp(volume, 0.0f, 1.0f);
            UpdateVolume();
        }

        // 소리 끄기
        public static void Mute()
        {
            _isMuted = true;
            UpdateVolume();
        }

        // 소리 켜기
        public static void Unmute()
        {
            _isMuted = false;
            UpdateVolume();
        }

        // 종료 처리 (OpenAL 리소스 해제)
        public static void Shutdown()
        {
            //Console.WriteLine("[DEBUG] 사운드 매니저 종료 중...");

            MusicPlayer.StopAudioProcessing(); // 플레이어 중지

            foreach (var buffer in EffectBuffers.Values)
            {
                AL.DeleteBuffer(buffer);
            }

            EffectBuffers.Clear();

            // OpenAL 리소스 해제
            var context = ALC.GetCurrentContext();
            var device = ALC.GetContextsDevice(context);
            if (context != IntPtr.Zero) ALC.DestroyContext(context);
            if (device != IntPtr.Zero) ALC.CloseDevice(device);

            Console.WriteLine("[DEBUG] 사운드 매니저 종료 완료.");
        }
    }
}