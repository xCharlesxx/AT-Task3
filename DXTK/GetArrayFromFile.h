#include <string>
#include <iostream>
#include <fstream>
#include <vector>
using namespace std;

class GetArrayFromFile
{
public:
	std::vector<char> GetVector(string fileLocation)
	{
		//Create vector of chars 
		std::vector<char> levelArray(5626, 0);
		std::ifstream file(fileLocation);

		if (!file.is_open())
		{
			MessageBox(
				NULL,
				(LPCWSTR)L"Error Cannot Open File",
				(LPCWSTR)L"Obtaining Level Structure",
				MB_ICONWARNING | MB_DEFBUTTON2);
		}
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
		}
		levelArray.erase(std::remove(levelArray.begin(), levelArray.end(), '\n'), levelArray.end());
		return levelArray;
	}

protected:
};
