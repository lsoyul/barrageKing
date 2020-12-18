using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameStatics
{
    public enum GAME_STATUS
    {
        SPLASH,
        MAINMENU,
        STAGE,
    }

    public enum SCENE_INDEX
    {
        LOADING = 0,
        MAINMENU = 1,
        GAMEPLAY = 2
    }

    public enum PLAYER_VIEW_MODE
    {
        TWO_DIMENSIONAL,            // 2D
        THREE_DIMENSIONAL           // 3D
    }

    public enum BULLET_TYPE
    {
        NONE,
        ARROW,
        HOLY,
        ICE,
        STONE,
        VOLCANO,
        ACC1,
    }

    public enum ENEMY_TYPE
    {
        NORMAL1
    }

    #region ##### Functions #####

    public static bool GetIsCollide(GameObject go1, GameObject go2, float collideDist, PLAYER_VIEW_MODE playerViewMode)
    {
        Vector2 vec2a;
        Vector2 vec2b;

        if (playerViewMode == PLAYER_VIEW_MODE.TWO_DIMENSIONAL)
        {
            // Only X, Z Comparison
            vec2a.x = go1.transform.position.x;
            vec2a.y = go1.transform.position.z;
            vec2b.x = go2.transform.position.x;
            vec2b.y = go2.transform.position.z;

            return (Vector2.Distance(vec2a, vec2b) < collideDist)? true : false;
            
        }
        else
        {
            return (Vector3.Distance(go1.transform.position, go2.transform.position) < collideDist) ? true : false;
        }
    }

    public static float GetBulletDamage(BULLET_TYPE bulletType)
    {
        switch (bulletType)
        {
            case BULLET_TYPE.ARROW:
                return 10;
            case BULLET_TYPE.HOLY:
                return 15;
            case BULLET_TYPE.ICE:
                return 5;
            case BULLET_TYPE.STONE:
                return 10;
            case BULLET_TYPE.VOLCANO:
                return 20;
            case BULLET_TYPE.ACC1:
                return 10;
            default:
                return 10;
        }
    }

    #endregion

}