using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Elevator : MonoBehaviour
{
    public TextMeshProUGUI currentFloorText;

    // quasi tout est en prive, mais tu peux les passer en public si tu veux les modifier
    float startingFloor = 3;
    float currentFloor;
    float minFloor =0;
    float maxFloor =3;

    float maxHeight = 3;
    float heightPerFloor;

    float moveTime = 1f;


    public AnimationCurve moveAnim;



    private void Start()
    {
        // ce code est utile que pour juste etre certain que l'ascenseur est place correctement au debut
        currentFloor = startingFloor;
        heightPerFloor = maxHeight / maxFloor;

        currentFloorText.text = (maxFloor - currentFloor).ToString();
        transform.localPosition = new Vector3(transform.localPosition.x, currentFloor* heightPerFloor, transform.localPosition.z);
    }



    // Les goup et godown sont trigger par des boutons. Dans ton cas, tu peux les trigger au meme endroit que tu triger les couleurs de tes piliers par exemple
    public void GoUp()
    {
        currentFloor += 1f;
        if (currentFloor > maxFloor)
            currentFloor = maxFloor;

        GoToFloor(currentFloor);

    }
    public void GoDown()
    {
        currentFloor -= 1f;
        if (currentFloor < minFloor)
            currentFloor = minFloor;

        GoToFloor(currentFloor);

    }


    void GoToFloor(float targetFloor)
    {
        StopAllCoroutines();
        StartCoroutine(MoveElevator(targetFloor));
    }

    IEnumerator MoveElevator(float targetFloor)
    {

        // ici je calcule jsute les infos de base
        float startTime = Time.time;
        float endTime = startTime + moveTime;
        float startPosition = transform.localPosition.y;
        float targetPosition = targetFloor * heightPerFloor;


        // le texte est juste la pour le feedback, tu peux le supprimer
        currentFloorText.text = (maxFloor - targetFloor).ToString();


        while (Time.time <= endTime)
        {
            // je le fais en plusieurs etapes

            // deja je regarde quelle est la proportion du temps ecoule. Si l'anim dure deux sec, et qu'il ya aeu 0.5 sec d'ecoulees, proportion sera egale a 25%
            float proportion = (Time.time - startTime) / moveTime;

            // ensuite je regarde sur ma courbe, a 25%, quelle est la hauteur de la courbe
            float eval = moveAnim.Evaluate(proportion);

            // et enfin, je calcule la position en y en fonction de la hauteur donnee par la courbe
            float position = startPosition + (targetPosition - startPosition) * eval;

            // et j'assigne
            transform.localPosition = new Vector3(transform.localPosition.x, position, transform.localPosition.z);


            yield return null;
        }

        // ici je remets la position a la position choisie juste pour etre sur que ce soit vraiment precis
        transform.localPosition = new Vector3(transform.localPosition.x, targetPosition, transform.localPosition.z);
        currentFloor = targetFloor;


        yield return null;
    }

}