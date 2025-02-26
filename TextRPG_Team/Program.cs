﻿using TextRPG_Team.Manager;

namespace TextRPG_Team;

using Scenes;

abstract class Program
{
    static void Main()
    {
        LoadManager.Load();
        
        GameState gameState = new GameState(); //인스턴스 생성
        
        // 첫 번째 씬 설정
        var initialScene = new TitleScene(gameState);

        var sceneManager = new SceneManager(initialScene);
        
        sceneManager.StartGame(); // 게임 시작


    }
}