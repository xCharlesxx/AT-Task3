#pragma once
#include "Sprite.h"

class SpriteFactory
{
public:
	SpriteFactory(ID3D11Device1* m_d3dDevice);
	~SpriteFactory();
	Sprite* Create(std::string _item, int posX, int posY) const;
private: 
	Microsoft::WRL::ComPtr<ID3D11ShaderResourceView> m_texture0;
	Microsoft::WRL::ComPtr<ID3D11ShaderResourceView> m_texture1;
	Microsoft::WRL::ComPtr<ID3D11ShaderResourceView> m_texture2;
	Microsoft::WRL::ComPtr<ID3D11ShaderResourceView> m_texture3;
	Microsoft::WRL::ComPtr<ID3D11ShaderResourceView> m_texture4;
	Microsoft::WRL::ComPtr<ID3D11ShaderResourceView> m_texture5;
	Microsoft::WRL::ComPtr<ID3D11ShaderResourceView> m_texture6;
	Microsoft::WRL::ComPtr<ID3D11ShaderResourceView> m_texture7;
	Microsoft::WRL::ComPtr<ID3D11ShaderResourceView> m_texture8;
	Microsoft::WRL::ComPtr<ID3D11ShaderResourceView> m_texture9;
	Microsoft::WRL::ComPtr<ID3D11ShaderResourceView> m_texWallDown;
	Microsoft::WRL::ComPtr<ID3D11ShaderResourceView> m_texWallDownLeft;
	Microsoft::WRL::ComPtr<ID3D11ShaderResourceView> m_texWallDownRight;
	Microsoft::WRL::ComPtr<ID3D11ShaderResourceView> m_texWallLeft;
	Microsoft::WRL::ComPtr<ID3D11ShaderResourceView> m_texWallMiddle;
	Microsoft::WRL::ComPtr<ID3D11ShaderResourceView> m_texWallRight;
	Microsoft::WRL::ComPtr<ID3D11ShaderResourceView> m_texWallUp;
	Microsoft::WRL::ComPtr<ID3D11ShaderResourceView> m_texWallUpLeft;
	Microsoft::WRL::ComPtr<ID3D11ShaderResourceView> m_texWallUpRight;

};

