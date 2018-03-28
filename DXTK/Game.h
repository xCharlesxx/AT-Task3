//
// Game.h
//

#pragma once

#include "StepTimer.h"
#include "SimpleMath.h"
#include "SpriteFactory.h"
#include "Level.h"

// A basic game implementation that creates a D3D11 device and
// provides a game loop.
class Game
{
public:

    Game();

    // Initialization and management
    void Initialize(HWND window, int width, int height);

    // Basic game loop
    void Tick();

    // Messages
    void OnActivated();
    void OnDeactivated();
    void OnSuspending();
    void OnResuming();
    void OnWindowSizeChanged(int width, int height);

    // Properties
    void GetDefaultSize( int& width, int& height ) const;

private:

    void Update(DX::StepTimer const& timer);
    void Render();

    void Clear();
    void Present();

    void CreateDevice();
    void CreateResources();

    void OnDeviceLost();
	//Messages 
	void OuchWall(); 
	void OuchFire(); 
    // Device resources.
    HWND                                            m_window;
    int                                             m_outputWidth;
    int                                             m_outputHeight;
    D3D_FEATURE_LEVEL                               m_featureLevel;
    Microsoft::WRL::ComPtr<ID3D11Device1>           m_d3dDevice;
    Microsoft::WRL::ComPtr<ID3D11DeviceContext1>    m_d3dContext;

    Microsoft::WRL::ComPtr<IDXGISwapChain1>         m_swapChain;
    Microsoft::WRL::ComPtr<ID3D11RenderTargetView>  m_renderTargetView;
    Microsoft::WRL::ComPtr<ID3D11DepthStencilView>  m_depthStencilView;

    // Rendering loop timer.
    DX::StepTimer                                   m_timer;

	Microsoft::WRL::ComPtr<ID3D11ShaderResourceView> m_texture; 
	std::unique_ptr<DirectX::SpriteBatch>            m_spriteBatch; 

	std::unique_ptr<Sprite>                          m_sprite; 

	// My Variables 
	vector<char>                                    m_currentLevel;
	SpriteFactory*                                  m_spriteFactory; 
	Level*                                          m_level;
	std::unique_ptr<DirectX::Keyboard>              m_keyboard;
	std::unique_ptr<DirectX::Mouse>                 m_mouse;
	float                                           tock = 0; 
	bool                                            UpKeyDown = false; 
	bool                                            DownKeyDown = false;
	bool                                            LeftKeyDown = false;
	bool                                            RightKeyDown = false;
};