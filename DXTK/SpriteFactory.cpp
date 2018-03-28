#include "pch.h"
#include "SpriteFactory.h"


SpriteFactory::SpriteFactory(ID3D11Device1* m_d3dDevice)
{
	SpriteLoaders::LoadPNGSprite(m_d3dDevice, m_texture0.ReleaseAndGetAddressOf(), L"Textures/Chest.png");
	SpriteLoaders::LoadPNGSprite(m_d3dDevice, m_texture1.ReleaseAndGetAddressOf(), L"Textures/Coin1.png");
	SpriteLoaders::LoadPNGSprite(m_d3dDevice, m_texture2.ReleaseAndGetAddressOf(), L"Textures/Coin2.png");
	SpriteLoaders::LoadPNGSprite(m_d3dDevice, m_texture3.ReleaseAndGetAddressOf(), L"Textures/Coin3.png");
	SpriteLoaders::LoadPNGSprite(m_d3dDevice, m_texture4.ReleaseAndGetAddressOf(), L"Textures/Coin4.png");
	SpriteLoaders::LoadPNGSprite(m_d3dDevice, m_texture5.ReleaseAndGetAddressOf(), L"Textures/Coin5.png");
	SpriteLoaders::LoadPNGSprite(m_d3dDevice, m_texture6.ReleaseAndGetAddressOf(), L"Textures/Coin6.png");
	SpriteLoaders::LoadPNGSprite(m_d3dDevice, m_texture7.ReleaseAndGetAddressOf(), L"Textures/Fire.png");
	SpriteLoaders::LoadPNGSprite(m_d3dDevice, m_texture8.ReleaseAndGetAddressOf(), L"Textures/Key.png");

	SpriteLoaders::LoadPNGSprite(m_d3dDevice, m_texWallDown.ReleaseAndGetAddressOf(),      L"Textures/Down.png");
	SpriteLoaders::LoadPNGSprite(m_d3dDevice, m_texWallDownLeft.ReleaseAndGetAddressOf(),  L"Textures/DownLeft.png");
	SpriteLoaders::LoadPNGSprite(m_d3dDevice, m_texWallDownRight.ReleaseAndGetAddressOf(), L"Textures/DownRight.png");
	SpriteLoaders::LoadPNGSprite(m_d3dDevice, m_texWallLeft.ReleaseAndGetAddressOf(),      L"Textures/Left.png");
	SpriteLoaders::LoadPNGSprite(m_d3dDevice, m_texWallMiddle.ReleaseAndGetAddressOf(),    L"Textures/Middle.png");
	SpriteLoaders::LoadPNGSprite(m_d3dDevice, m_texWallRight.ReleaseAndGetAddressOf(),     L"Textures/Right.png");
	SpriteLoaders::LoadPNGSprite(m_d3dDevice, m_texWallUp.ReleaseAndGetAddressOf(),        L"Textures/Up.png");
	SpriteLoaders::LoadPNGSprite(m_d3dDevice, m_texWallUpLeft.ReleaseAndGetAddressOf(),    L"Textures/UpLeft.png");
	SpriteLoaders::LoadPNGSprite(m_d3dDevice, m_texWallUpRight.ReleaseAndGetAddressOf(),   L"Textures/UpRight.png");
}


SpriteFactory::~SpriteFactory()
{
}

Sprite * SpriteFactory::Create(std::string _item, int posX, int posY) const
{
	Sprite* m_sprite = new Sprite();
	switch (_item[0])
	{
	case 'P':
		m_sprite->CreateSprite(m_texture0, "Player", posX, posY);
		return m_sprite; 
		break;
	case 'L':
		m_sprite->CreateSprite(m_texture7, "Lava", posX, posY);
		return m_sprite;
		break;
	case 'C':
		m_sprite->CreateSprite(m_texture6, "Coin", posX, posY);
		m_sprite->setTextureFrames(m_texture1, m_texture2, m_texture3, m_texture4, m_texture5, m_texture6);
		return m_sprite;
		break;
	case '1':
		m_sprite->CreateSprite(m_texWallDownRight, "WallDownRight", posX, posY);
		return m_sprite;
		break;
	case '2':
		m_sprite->CreateSprite(m_texWallDownLeft, "WallDownLeft", posX, posY);
		return m_sprite;
		break;
	case '3':
		m_sprite->CreateSprite(m_texWallDown, "WallDown", posX, posY);
		return m_sprite;
		break;
	case '4':
		m_sprite->CreateSprite(m_texWallRight, "WallRight", posX, posY);
		return m_sprite;
		break;
	case '5':
		m_sprite->CreateSprite(m_texWallLeft, "WallLeft", posX, posY);
		return m_sprite;
		break;
	case '6':
		m_sprite->CreateSprite(m_texWallUpRight, "WallUpRight", posX, posY);
		return m_sprite;
		break;
	case '7':
		m_sprite->CreateSprite(m_texWallUpLeft, "WallUpLeft", posX, posY);
		return m_sprite;
		break;
	case '8':
		m_sprite->CreateSprite(m_texWallUp, "WallUp", posX, posY);
		return m_sprite;
		break;
	default:
		m_sprite->CreateSprite(m_texWallMiddle, "WallMiddle", posX, posY);
		return m_sprite;
		return nullptr;
	}
}
