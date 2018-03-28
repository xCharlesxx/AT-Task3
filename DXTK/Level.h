#pragma once
#include "GetArrayFromFile.h"

class Level
{
public:
	Level();
	~Level();
	vector<char> GetLevel(SpriteFactory* SF);
	void PrintLevel(vector<char>& level);
	void MovePlayerRight() { m_playerPos -= 1; };
	void MovePlayerLeft()  { m_playerPos += 1; };
	void MovePlayerUp()    { m_playerPos += m_levelWidth; };
	void MovePlayerDown()  { m_playerPos -= m_levelWidth; };
	std::wstring ScoreAsWstring(int score);
	vector<Sprite*> visibleSprites; 
	int score = 0; 
	int totalScore = 0; 
	int m_playerPos;
	int m_levelWidth = 75;
	int m_levelHeight = 75;
	int levelNum = 1;
private:
	void Search(int startPos, int pos, char wall, int direction, vector<char> m_tempLevel, vector<int> &corridorsInView);
	void GetWalls(int pos, vector<char> m_tempLevel, vector<int> &wallsToDraw);
	bool NoWallUp(int pos, vector<char> m_tempLevel);
	bool NoWallDown(int pos, vector<char> m_tempLevel);
	bool NoWallLeft(int pos, vector<char> m_tempLevel);
	bool NoWallRight(int pos, vector<char> m_tempLevel);
	int GetX(int pos); 
	int GetY(int pos); 
	string gameName = "New Game";

	SpriteFactory * m_SF; 
	const char wall = '0'; 
};

