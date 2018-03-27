#include "pch.h"
#include "SpriteFactory.h"
#include "Level.h"


Level::Level()
{
}


Level::~Level()
{
}

vector<char> Level::GetLevel(SpriteFactory* SF)
{
	string m_fileLocation = "..\\LevelMaker\\LevelMaker\\Levels\\New Game\\1";;
	GetArrayFromFile item; 
	vector<char> level; 
	level = item.GetVector(m_fileLocation); 
	m_SF = SF; 
	int startX = m_levelWidth / 2; 
	int startY = m_levelHeight / 2; 
	m_playerPos = (startY * m_levelHeight) + startX;
	PrintLevel(level);
	return level; 
}

void Level::PrintLevel(vector<char> level)
{
	visibleSprites.clear(); 
	vector<char> m_tempLevel = level;
	vector<int> corridorsInView;
	vector<int> wallsToDraw;
	if (m_tempLevel[m_playerPos] == wall)
		MessageBox(
			NULL,
			(LPCWSTR)L"Error Player Spawned in Wall",
			(LPCWSTR)L"Printing Level",
			MB_ICONWARNING | MB_DEFBUTTON2);
	int playerXPos = m_playerPos % m_levelWidth;
	int playerYPos = m_playerPos / m_levelWidth;
	visibleSprites.push_back(m_SF->Create("0", 400, 300));
	//Gets all corridors in view
	Search(m_playerPos, m_playerPos, wall, 1,             m_tempLevel, corridorsInView);
	Search(m_playerPos, m_playerPos, wall, -1,            m_tempLevel, corridorsInView);
	Search(m_playerPos, m_playerPos, wall, m_levelWidth,  m_tempLevel, corridorsInView);
	Search(m_playerPos, m_playerPos, wall, -m_levelWidth, m_tempLevel, corridorsInView);

	//Gets all the walls surrounding the corridors in view
	for (int i = 0; i < corridorsInView.size(); i++)
		GetWalls(corridorsInView[i], m_tempLevel, wallsToDraw);

	//Removes Duplicates 
	sort(wallsToDraw.begin(), wallsToDraw.end());
	wallsToDraw.erase(unique(wallsToDraw.begin(), wallsToDraw.end()), wallsToDraw.end());

	//Adds walls to draw list
	for (int i = 0; i < wallsToDraw.size(); i++)
	{
		int relativeXPos = playerXPos - (wallsToDraw[i] % m_levelWidth); 
		int relativeYPos = playerYPos - (wallsToDraw[i] / m_levelWidth);
		string spriteTexture; 
		switch (m_tempLevel[wallsToDraw[i]])
		{
		case '0':
			spriteTexture = "1";
			break;
		case 'T':
			spriteTexture = "2";
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

void Level::Search(int startPos, int pos, char wall, int direction, vector<char> m_tempLevel, vector<int> &corridorsInView)
{
	if (m_tempLevel[pos] == wall)
		return; 

	corridorsInView.push_back(pos); 

	Search(startPos, pos + direction, wall, direction, m_tempLevel, corridorsInView);
}

void Level::GetWalls(int pos, vector<char> m_tempLevel, vector<int> &wallsToDraw)
{
	for (int dRow = -m_levelWidth; dRow <= m_levelWidth; dRow += m_levelWidth)
		for (int dCol = -1; dCol <= 1; ++dCol)
			//if (dRow == 0 || dCol == 0)
				if (m_tempLevel[pos + dRow + dCol] == wall || m_tempLevel[pos + dRow + dCol] == 'T')
					wallsToDraw.push_back(pos + dRow + dCol); 
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