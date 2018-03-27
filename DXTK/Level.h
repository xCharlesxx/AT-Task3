#pragma once
#include "GetArrayFromFile.h"

class Level
{
public:
	Level();
	~Level();
	vector<char> GetLevel(SpriteFactory* SF);
	void PrintLevel(vector<char> level);
	void MovePlayerRight() { m_playerPos -= 1; };
	void MovePlayerLeft()  { m_playerPos += 1; };
	void MovePlayerUp()    { m_playerPos += m_levelWidth; };
	void MovePlayerDown()  { m_playerPos -= m_levelWidth; };
	vector<Sprite*> visibleSprites; 
private:
	void Search(int startPos, int pos, char wall, int direction, vector<char> m_tempLevel, vector<int> &corridorsInView);
	void GetWalls(int pos, vector<char> m_tempLevel, vector<int> &wallsToDraw);
	SpriteFactory * m_SF; 
	int m_playerPos; 
	int m_levelWidth = 75; 
	int m_levelHeight = 75; 
	const char wall = '0'; 
};

