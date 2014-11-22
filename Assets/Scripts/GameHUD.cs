﻿using UnityEngine;
using System.Collections;

public class GameHUD : MonoBehaviour {

	GameManager gameManager;
	PlayerScript playerScript;

	Texture2D healthBar;
	Texture2D healthBarFill;
	Texture2D gainsBar;
	Texture2D gainsBarFill;

	void Start () 
	{
		// cache references
		gameManager = GetComponent<GameManager>();
		playerScript = GameObject.Find("Lucina").GetComponent<PlayerScript>();

		healthBar = Resources.Load("Textures/health-bar-with-highlights") as Texture2D;
		healthBarFill = CreateHealthBarTexture();
		gainsBar = Resources.Load("Textures/gains-bar-with-highlights") as Texture2D;
		gainsBarFill = CreateGainsBarTexture();
	}
	
	void Update () {
	
	}

	private Color GetColorFrom256Scale(int r, int g, int b) {
		return new Color(r/255f, g/255f, b/255f);
	}
	
	private Color GetColorFrom256Scale(int r, int g, int b, float a) {
		return new Color(r/255f, g/255f, b/255f, a);
	}

	private Texture2D CreateHealthBarTexture() {
		int width = healthBar.width;
		int height = healthBar.height;
		
		Texture2D hBarFill = new Texture2D(width, height);
		Color[] colors = hBarFill.GetPixels();
		for (int i = 0; i < hBarFill.height; i++) {
			for (int j = 0; j < hBarFill.width; j++) {
				if (hBarFill.height-1-12 <= i && i <= hBarFill.height-1-10) {
					colors[hBarFill.width * i + j] = GetColorFrom256Scale(93, 243, 199);
				} else if (hBarFill.height-1-33 <= i && i <= hBarFill.height-1-5) {
					colors[hBarFill.width * i + j] = GetColorFrom256Scale(24, 182, 146);
				} else {
					colors[hBarFill.width * i + j] = GetColorFrom256Scale(12, 156, 114);
				}
			}
		}
		hBarFill.SetPixels(colors);
		hBarFill.Apply();

		return hBarFill;
	}

	private Texture2D CreateGainsBarTexture() {
		int width = gainsBar.width;
		int height = gainsBar.height;
		
		Texture2D gBarFill = new Texture2D(width, height);
		Color[] colors = gBarFill.GetPixels();

		for (int i = 0; i < gBarFill.height; i++) {
			for (int j = 0; j < gBarFill.width; j++) {
				if (10 <= j && j <= 12) {
					colors[gBarFill.width * i + j] = GetColorFrom256Scale(253, 185, 205, 0.65f);
				} else if (5 <= j && j <= 33) {
					colors[gBarFill.width * i + j] = GetColorFrom256Scale(210, 44, 94, 0.65f);
				} else {
					colors[gBarFill.width * i + j] = GetColorFrom256Scale(177, 6, 57, 0.65f);
				}
			}
		}
		gBarFill.SetPixels(colors);
		gBarFill.Apply();
		
		return gBarFill;
	}

	// Right now this is hooked to num lives, will have to change later
	private void DrawHealthBar() {
		float healthRatio = (float)gameManager.GetTotalLives() / gameManager.GetMaxLives();

		// Fraction of the screen width we want the health bar to take up
		float mult = 0.35f;

		// New width and height of health bar on screen w.r.t our Screen size
		int w = (int)(Screen.width * mult);
		int h = (int)(w * healthBar.height / healthBar.width);

		// Bar border thickness in vert and horiz directions. Need to calculate due to resizing from original size.
		// 7 comes from the full size border being 7 pixels. +1 because it usually looks better with that buffer.
		int bw = (int)((float)h / healthBar.height * 7) + 1;
		int bh = (int)((float)w /healthBar.width * 7) + 1;

		GUI.BeginGroup(new Rect(Screen.width - w - 10, (int)(Screen.height * 0.03f), w, h));
			GUI.DrawTexture(new Rect(0, 0, w, h), healthBar);
			GUI.DrawTexture(new Rect(bw, bh, (int)((w - bw * 2) * healthRatio), h - bh * 2), healthBarFill); 
		GUI.EndGroup();
	}

	private void DrawGainsBar() {
		float gainsRatio = (float)playerScript.GetCurrentPowerGain() / playerScript.GetCurrentPowerMaxGain();
		
		// Fraction of the screen width we want the gains bar to take up
		float mult = 0.65f;
		
		// New width and height of gains bar on screen w.r.t our Screen size
		int h = (int)(Screen.height * mult);
		int w = (int)(h * gainsBar.width / gainsBar.height);
		
		// Bar border thickness in vert and horiz directions. Need to calculate due to resizing from original size.
		// 7 comes from the full size border being 7 pixels. +1 because it usually looks better with that buffer.
		int bw = (int)((float)h / gainsBar.height * 7) + 1;
		int bh = (int)((float)w /gainsBar.width * 7) + 1;

		int fillHeight = (int)((h - bh * 2) * gainsRatio);
		GUI.BeginGroup(new Rect(10, (int)(Screen.height * 0.2f), w, h));
			GUI.DrawTexture(new Rect(0, 0, w, h), gainsBar);
			GUI.DrawTexture(new Rect(bw, h - bh - fillHeight, w - bw * 2, fillHeight), gainsBarFill); 
		GUI.EndGroup();
	}

	void OnGUI() 
	{
		GUI.Label(new Rect(50, 15, Screen.width / 5, Screen.height / 10), "Lives: " + gameManager.GetTotalLives());

		// Loss state
		if (gameManager.GetHasLost()) 
		{
			GUI.Label(new Rect(Screen.width / 2, Screen.height / 2, Screen.width / 2, Screen.height / 2), "LUCINA IS DEAD. GAME OVER");
		}
		if (gameManager.GetHasWon())
		{
			GUI.Label(new Rect(Screen.width / 2, Screen.height / 2, Screen.width / 2, Screen.height / 2), "MAGNET MASTER HAS BEEN AVENGED. YOU WIN");
		}

		// Powers
		GUI.Label(new Rect(50, 45, Screen.width / 5, Screen.height / 10), playerScript.GetCurrentPower().ToString());
		GUI.Label(new Rect(50, 75, Screen.width / 5, Screen.height / 10), "GAIN: " + playerScript.GetCurrentPowerGain());

		// Health
		DrawHealthBar();
		DrawGainsBar();
	}
}
