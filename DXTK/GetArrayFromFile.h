#include <string>
#include <iostream>
#include <fstream>
#include <vector>
using namespace std;

class GetArrayFromFile
{
public:
	int gridWidth = 0; 
	int gridHeight = 0; 
	std::vector<char> GetVector(string fileLocation)
	{
		std::ifstream file(fileLocation);
		if (!file.is_open())
		{
			MessageBox(
				NULL,
				(LPCWSTR)L"Error Cannot Open File",
				(LPCWSTR)L"Obtaining Level Structure",
				MB_ICONWARNING | MB_DEFBUTTON2);
			PostQuitMessage(0);
		}
		//Create vector of chars (max 75x75)
		std::vector<char> levelArray((gridHeight * gridWidth) + 1, 0);
		//Whilst not end of file, print file to vector 
		if (!file.eof())
		{
			levelArray.assign((std::istreambuf_iterator<char>(file)),
			                  std::istreambuf_iterator<char>());
		}
		else
		{
			MessageBox(
				NULL,
				(LPCWSTR)L"Error Reading from File",
				(LPCWSTR)L"Obtaining Level Structure",
				MB_ICONWARNING | MB_DEFBUTTON2);
			PostQuitMessage(0);
		}
		//Get gridWidth and gridHeight
		for (int i = 0; i < levelArray.size(); i++)
		{
			if (levelArray[i] == '\n')
			{
				gridHeight++;
				gridWidth = 0;
			}
			gridWidth++;
		}
		gridWidth--; 
		gridHeight++; 

		//Remove all new line chars
		levelArray.erase(std::remove(levelArray.begin(), levelArray.end(), '\n'), levelArray.end());
		levelArray.resize((gridHeight*gridWidth) + 1);
		return levelArray;
	}

protected:
};
