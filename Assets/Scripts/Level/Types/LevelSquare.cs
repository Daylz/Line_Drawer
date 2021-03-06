﻿using UnityEngine;
using System.Collections;

public class LevelSquare : ALevel
{
	public LevelSquare()
	{
		Tiles = new int[9, 9]{
                               { 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                               { 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                               { 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                               { 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                               { 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                               { 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                               { 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                               { 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                               { 1, 1, 1, 1, 1, 1, 1, 1, 1 }
                               };

		Spawners = new int[9, 9]{
                               { 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                               { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                               { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                               { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                               { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                               { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                               { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                               { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                               { 0, 0, 0, 0, 0, 0, 0, 0, 0 }
                               };

        Gravity = new int[9, 9]{
                               { 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                               { 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                               { 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                               { 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                               { 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                               { 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                               { 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                               { 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                               { 1, 1, 1, 1, 1, 1, 1, 1, 1 }
                               };
    }
}
