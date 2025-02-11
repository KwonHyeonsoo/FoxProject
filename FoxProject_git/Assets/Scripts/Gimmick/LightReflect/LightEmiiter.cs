using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public struct Line { public Vector3 start, end; }   //start �ʿ� ���� end�� ������

[RequireComponent(typeof(LightController))]
public class LightEmiiter : MonoBehaviour
{
    public LayerMask mask;

    public List<Line> lasers = new List<Line>();

    public GameObject laserObject;
    LineRenderer LRender;
    LightController controller;
    light_type type;

    LightController Old_receiver;
    // Start is called before the first frame update
    void Start()
    {
        LRender = laserObject.GetComponent<LineRenderer>();
        controller = GetComponent<LightController>();
        type = controller.type = light_type.emitter;

        mask = LayerMask.GetMask("Map");
        reRaycast();

    }

    private void FixedUpdate()
    {
        reRaycast();
        DrawLaser();
    }
    void reRaycast()
    {
        Ray ray = new Ray();
        RaycastHit hit;
        LightController component;

        ray.origin = transform.position;
        ray.direction = transform.forward;

        lasers.Clear();

        Physics.Raycast(ray, out hit, 100f, mask);
        Debug.DrawLine(ray.origin, hit.point, UnityEngine.Color.red);
        
        while (hit.collider != null)    
        {
            Debug.Log("reRaycast");
            component = hit.transform.GetComponent<LightController>();

            if (Old_receiver) { Old_receiver.LightUntrigger(hit); }

            if (component != null)
            {
                Line tmp = new Line();
                tmp.start = ray.origin;
                tmp.end = hit.point;
                lasers.Add(tmp);

                if (component.getLightType() == light_type.reflector)
                {
                    Vector3 incomeVec = hit.point - ray.origin;
                    Debug.DrawLine(ray.origin, hit.point, UnityEngine.Color.blue);  //�Լ� ����
                    Vector3 norVec = hit.normal;
                    Debug.DrawRay(hit.point, hit.normal, UnityEngine.Color.green);  //�븻 ����
                    ray.direction =  Vector3.Reflect(incomeVec, norVec);
                    ray.origin = hit.point;

                    Physics.Raycast(ray, out hit, 100f, mask);
                    Debug.DrawRay(ray.origin,ray.direction, UnityEngine.Color.red);

                    //Debug.Log(
                    //    "�Լ�:" + incomeVec + "�븻:" + norVec + "�ݻ�:" + ray.direction);
                }
                else if (component.getLightType() == light_type.receiver)
                {
                    component.LightTrigger(hit);
                    Old_receiver = component;
                    return; 
                }
                else//��θ� �����ϴ� ��ü�� �ε����� ��
                {
                    return;
                }
            }
            else//��θ� �����ϴ� ��ü�� �ε����� ��
            {
                Line tmp = new Line();
                tmp.start = ray.origin;
                tmp.end = hit.point;
                lasers.Add(tmp);
                return;
            }

        }
        if (Old_receiver) { Old_receiver.LightUntrigger(hit); }
        //�ε����� �ʾ��� �� //���� ���� ���ձ�
        Line a = new Line();
        a.start = ray.origin;
        a.end = ray.origin + ray.direction * 10;
        lasers.Add(a);

    }

    private void DrawLaser()
    {
        if (lasers.Count > 0)
        {
            laserObject.SetActive(true);
            Vector3 rotateVector = lasers[0].end - lasers[0].start;

            laserObject.transform.rotation = quaternion.Euler (0, Quaternion.Euler(rotateVector).y, 0);
            //laserObject.transform.eulerAngles = rotateVector;
            //Debug.Log(rotateVector +" "+ quaternion.Euler(0, Quaternion.Euler(rotateVector).y, 0));
            Vector3 initOffset = lasers[0].start;   //��ġ ����

            LRender.positionCount = lasers.Count + 1;
            LRender.SetPosition(0, lasers[0].start - initOffset);
            for (int i = 0; i < lasers.Count; i++)
            {
                LRender.SetPosition(i+1, lasers[i].end - initOffset);
  
            }
        }
        else laserObject.SetActive(false);
    }
    
}
