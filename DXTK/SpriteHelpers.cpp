/*Copyright(c) 2016 RafalRebisz

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files(the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and / or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions :

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.*/

#include "pch.h"

using Microsoft::WRL::ComPtr;

namespace SpriteLoaders
{
	HRESULT LoadDDSSprite(ID3D11Device* directXDevice, ID3D11ShaderResourceView** texture, std::wstring fileName)
	{
		// Create texture and resource
		return DirectX::CreateDDSTextureFromFile(directXDevice, fileName.c_str(), nullptr, texture);
	}

	HRESULT LoadPNGSprite(ID3D11Device* directXDevice, ID3D11ShaderResourceView** texture, std::wstring fileName)
	{
		// Create texture and resource
		return DirectX::CreateWICTextureFromFile(directXDevice, fileName.c_str(), nullptr, texture);
	}
}