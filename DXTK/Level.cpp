#include "pch.h"
#include "SpriteFactory.h"
#include <iostream>
#include <fstream>
#include "Level.h"


Level::Level()
{
	//Get level to use from GameName
	std::ifstream game("..\\LevelMaker\\LevelMaker\\GameName.txt");
	std::string temp((std::istreambuf_iterator<char>(game)), std::istreambuf_iterator<char>()); 
	gameName = temp; 
}


Level::~Level()
{
}

vector<char> Level::GetLevel(SpriteFactory* SF)
{
	string m_fileLocation = "..\\LevelMaker\\LevelMaker\\Levels\\" + gameName + "\\" + to_string(levelNum);
	GetArrayFromFile item; 
	vector<char> level; 
	level = item.GetVector(m_fileLocation); 
	m_SF = SF; 
	int startX = m_levelWidth / 2; 
	int startY = m_levelHeight / 2; 
	m_playerPos = (startY * m_levelHeight) + startX;
	PrintLevel(level);
	for (int i = 0; i < level.size(); i++)
		if (level[i] == 'T')
			totalScore++; 

	MessageBox(
		NULL,
		(LPCWSTR)L"Chester the Treasure Chest has lost his Treasure, Help Him Find it!\nPress 'S' to See How Many Are Left!",
		(LPCWSTR)L"Chester's Fable",
		MB_ICONINFORMATION | MB_DEFBUTTON2);

	return level; 
}

void Level::PrintLevel(vector<char>& level)
{
	visibleSprites.clear(); 
	vector<int> corridorsInView;
	vector<int> wallsToDraw;
	if (level.size() == 0)
		return; 
	if (level[m_playerPos] == wall)
		MessageBox(
			NULL,
			(LPCWSTR)L"Error Player Spawned in Wall",
			(LPCWSTR)L"Printing Level",
			MB_ICONWARNING | MB_DEFBUTTON2);
	if (level[m_playerPos] == 'T')
	{
		level[m_playerPos] = '1';
		score++; 
		
		/*MessageBox(
			NULL,
			(LPCWSTR)ScoreAsWstring(totalScore - score).c_str(),
			(LPCWSTR)L"Score",
			MB_ICONWARNING | MB_DEFBUTTON2);*/
	}
	int playerXPos = GetX(m_playerPos); 
	int playerYPos = GetY(m_playerPos);
	//Draw Player
	visibleSprites.push_back(m_SF->Create("Player", 400, 300));
	//Gets all corridors in view
	Search(m_playerPos, m_playerPos, wall, 1,             level, corridorsInView);
	Search(m_playerPos, m_playerPos, wall, -1,            level, corridorsInView);
	Search(m_playerPos, m_playerPos, wall, m_levelWidth,  level, corridorsInView);
	Search(m_playerPos, m_playerPos, wall, -m_levelWidth, level, corridorsInView);

	//Gets all the walls surrounding the corridors in view
	for (int i = 0; i < corridorsInView.size(); i++)
		GetWalls(corridorsInView[i], level, wallsToDraw);

	//Removes Duplicates 
	sort(wallsToDraw.begin(), wallsToDraw.end());
	wallsToDraw.erase(unique(wallsToDraw.begin(), wallsToDraw.end()), wallsToDraw.end());

	//Adds walls to draw list
	for (int i = 0; i < wallsToDraw.size(); i++)
	{
		int relativeXPos = playerXPos - (wallsToDraw[i] % m_levelWidth); 
		int relativeYPos = playerYPos - (wallsToDraw[i] / m_levelWidth);
		string spriteTexture = "X"; 
		switch (level[wallsToDraw[i]])
		{
			case '0':
				//If Tile is Below or Right of player
				if (m_playerPos < wallsToDraw[i])
				{
					if (NoWallUp(wallsToDraw[i], level) && NoWallLeft(wallsToDraw[i], level))
						spriteTexture = "1";
					else if (NoWallUp(wallsToDraw[i], level) && NoWallRight(wallsToDraw[i], level))
						spriteTexture = "2"; 
					else if (NoWallUp(wallsToDraw[i], level))
						spriteTexture = "3";
				}
				//If tile is Above or Left of player
				else
				{
					if (NoWallDown(wallsToDraw[i], level) && NoWallLeft(wallsToDraw[i], level))
						spriteTexture = "6";
					else if (NoWallDown(wallsToDraw[i], level) && NoWallRight(wallsToDraw[i], level))
						spriteTexture = "7";
					else if (NoWallDown(wallsToDraw[i], level))
						spriteTexture = "8";
				}
				if (spriteTexture == "X")
				{
					int a, b, c; 
					a = GetX(m_playerPos); 
					b = (m_levelWidth * m_levelHeight) - (m_levelWidth - GetX(m_playerPos));
					c = wallsToDraw[i]; 
					//Check if left of player 
					if (((GetX(b) - GetX(a))*(GetY(c) - GetY(a)) - (GetY(b) - GetY(a))*(GetX(c) - GetX(a))) > 0)
						spriteTexture = "5";
					else 
						spriteTexture = "4"; 
				}
				break;
			case 'T':
				spriteTexture = "Coin";
				break;
			case 'L':
				spriteTexture = "Lava";
				break;
			default:
				break;
		}
		visibleSprites.push_back(m_SF->Create(spriteTexture, 400 + relativeXPos * 60, 300 + relativeYPos * 60));
	}

	//Sprite * m_sprite;
	//m_sprite = m_SF->Create("0Wall", 400, 300); 
	//visibleSprites.push_back(m_sprite); 
}

void Level::Search(int startPos, int pos, char wall, int direction, vector<char> level, vector<int> &corridorsInView)
{
	if (level[pos] == wall)
		return; 

	corridorsInView.push_back(pos); 

	Search(startPos, pos + direction, wall, direction, level, corridorsInView);
}

void Level::GetWalls(int pos, vector<char> level, vector<int> &wallsToDraw)
{
	//Check for walls or Treasure surrounding player
	for (int dRow = -m_levelWidth; dRow <= m_levelWidth; dRow += m_levelWidth)
		for (int dCol = -1; dCol <= 1; ++dCol)
				if (level[pos + dRow + dCol] == wall || level[pos + dRow + dCol] == 'T' || level[pos + dRow + dCol] == 'L')
					wallsToDraw.push_back(pos + dRow + dCol); 
}

bool Level::NoWallUp(int pos, vector<char> level)
{
	if (level[pos - m_levelWidth] == wall)
		return false;
	return true;
}

bool Level::NoWallDown(int pos, vector<char> level)
{
	if (level[pos + m_levelWidth] == wall)
		return false;
	return true;
}

bool Level::NoWallLeft(int pos, vector<char> level)
{
	if (level[pos - 1] == wall)
		return false;
	return true;
}

bool Level::NoWallRight(int pos, vector<char> level)
{
	if (level[pos + 1] == wall)
		return false;
	return true;
}

int Level::GetX(int pos)
{
	return pos % m_levelWidth;
}

int Level::GetY(int pos)
{
	return pos / m_levelWidth;
}

std::wstring Level::ScoreAsWstring(int score)
{
	LPCWSTR temp = L"Only ";
	wchar_t buffer[256];
	wsprintfW(buffer, L"%d", score);
	LPCWSTR temp2 = buffer;
	LPCWSTR temp3 = L" To Go!";
	std::wstring combine = std::wstring(temp) + temp2;
	std::wstring combine2 = std::wstring(combine) + temp3;
	return combine2;
}



//int row = 0;
//int column = 0;
////Get char and print tile associated with that char
//for (vector<char>::iterator it = level.begin(); it != level.end(); ++it)
//{
//	Sprite* m_sprite;
//	switch (static_cast<char>(*it))
//	{
//	case '0':
//		//Wall
//		m_sprite = m_SF->Create("0Wall");
//		break;
//	case '1':
//		//Corridor
//		break;
//	case 'L':
//		//Lava
//		break;
//	case 'D':
//		//Door
//		break;
//	case 'T':
//		//Tresure
//		break;
//	case '\n':
//		row++;
//		column = 0;
//		continue;
//	case '\r':
//		continue;
//	}
//	column++;
//}