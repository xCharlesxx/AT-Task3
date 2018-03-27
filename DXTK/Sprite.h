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

#pragma once
#include "SimpleMath.h"


///////////////////////////////////////////////
// Class : Sprite		Author : Rafal Rebisz
///////////////////////////////////////////////
//
// Purpose : Provides Simple Interface for loading
//			 and rendering of game sprites
//
///////////////////////////////////////////////

class Sprite
{
public: // Methods

	// Constructor
	Sprite(void);
	// Copy constructor
	Sprite(const Sprite& other);
	// overloaded constructor
	Sprite(const Microsoft::WRL::ComPtr<ID3D11ShaderResourceView>& preAllocatedTexture, std::string ID, int posX, int posY);
	// Destructor
	virtual ~Sprite(void);
	// Overloaded assignment operator 
	const Sprite& operator =(const Sprite& other);
	void setTextureFrames(const Microsoft::WRL::ComPtr<ID3D11ShaderResourceView>& tex1,
		const Microsoft::WRL::ComPtr<ID3D11ShaderResourceView>& tex2,
		const Microsoft::WRL::ComPtr<ID3D11ShaderResourceView>& tex3,
		const Microsoft::WRL::ComPtr<ID3D11ShaderResourceView>& tex4,
		const Microsoft::WRL::ComPtr<ID3D11ShaderResourceView>& tex5,
		const Microsoft::WRL::ComPtr<ID3D11ShaderResourceView>& tex6)
	{
		m_tex1 = tex1; 
		m_tex2 = tex2;
		m_tex3 = tex3;
		m_tex4 = tex4;
		m_tex5 = tex5;
		m_tex6 = tex6;
		isAnimated = true; 
	}
	// Creates sprite 
	// Use if object instantiated by default constructor 
	void CreateSprite(const Microsoft::WRL::ComPtr<ID3D11ShaderResourceView>& preAllocatedTexture, std::string ID, int posX, int posY);
	void Draw(DirectX::SpriteBatch*);
	// Render
	virtual void Render(DirectX::SpriteBatch* spriteBatch, const DirectX::XMFLOAT2 position, /*future parameters are optional*/
						float rotation = SpriteConstants::ROTATION_ZERO, float depth = SpriteConstants::DEPTH_ZERO, const RECT* sourceRect = nullptr,
						const DirectX::XMFLOAT2& origin = SpriteConstants::SPRITE_ORIGIN, const DirectX::XMFLOAT2& scale = SpriteConstants::SPRITE_SCALE,
						DirectX::SpriteEffects effect = DirectX::SpriteEffects_None, DirectX::XMVECTOR color = DirectX::Colors::White) const
	{
		if (m_texture)
		{
			spriteBatch->Draw(m_texture.Get(), position, sourceRect, color, rotation, origin, scale, effect, depth);
		}
		else
		{
			throw std::exception();
		}
	}

	// Get Sprite dimensions
	virtual int GetSpriteWidth(void) const				{ return m_width; }
	virtual int GetSpriteHeight(void) const				{ return m_height; }

	// Get Sprite ID
	virtual const std::string& GetSpriteID(void) const	{ return m_spriteID; }
	virtual std::string& GetSpriteID(void)				{ return m_spriteID; }
	virtual void UpdateTexture(); 
	bool isAnimated = false;
protected: //  Members 

	// pointer to ID3D11ShaderResourceView
	// that contains texture buffer
	Microsoft::WRL::ComPtr<ID3D11ShaderResourceView> m_texture;

	// Sprite dimension
	unsigned int m_height;
	unsigned int m_width;
	const int spritePixelSize = 60; 

	DirectX::XMFLOAT2 m_pos = DirectX::XMFLOAT2(0.0f, 0.0f);
	// Sprite ID
	std::string m_spriteID;
	Microsoft::WRL::ComPtr<ID3D11ShaderResourceView> m_tex1;
	Microsoft::WRL::ComPtr<ID3D11ShaderResourceView> m_tex2;
	Microsoft::WRL::ComPtr<ID3D11ShaderResourceView> m_tex3;
	Microsoft::WRL::ComPtr<ID3D11ShaderResourceView> m_tex4;
	Microsoft::WRL::ComPtr<ID3D11ShaderResourceView> m_tex5;
	Microsoft::WRL::ComPtr<ID3D11ShaderResourceView> m_tex6;
};