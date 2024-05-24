using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;

public class FireManager : MonoBehaviour
{
    [SerializeField] private BulletPooler bulletPooler;
    [SerializeField] private BulletPooler followBulletPooler;
    [SerializeField] private Transform basePos;
    [SerializeField] private int bulletNum;
    [SerializeField] private int followNum;
    private Vector3 pos;

    public void Fire()
    {
        int angle = 360 / bulletNum;
        int angleCount = angle;
        for (int i = 0; i < 360; i += angleCount)
        {
            pos = new Vector3(
                Mathf.Cos(Mathf.Deg2Rad * angle),
                basePos.position.y,
                Mathf.Sin(Mathf.Deg2Rad * angle));

            bulletPooler.PullBullet(pos, (pos - basePos.position).normalized);
            angle += angleCount;
        }
    }

    public void FireFollow()
    {
        int angle = 360 / followNum;
        int angleCount = angle;
        for (int i = 0; i < 360; i += angleCount)
        {
            pos = new Vector3(
                Mathf.Cos(Mathf.Deg2Rad * angle),
                basePos.position.y,
                Mathf.Sin(Mathf.Deg2Rad * angle));

            followBulletPooler.PullBullet(pos, (pos - basePos.position).normalized);
            angle += angleCount;
        }
    }
}
