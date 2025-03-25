using UnityEngine;
using System;
using UnityEngine.EventSystems;

public class MonsterDrop : MonoBehaviour
{
    public GameObject monsterSaved;
    private bool isMonsterSelected = false; // Bandera para saber si ya se eligió un monstruo

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Detecta clic izquierdo
        {
            // Si ya hay un monstruo guardado, intentar instanciarlo
            if (isMonsterSelected)
            {
                SpawnMonster();
            }
        }
    }

    // Método para seleccionar el monstruo al hacer clic en la imagen
    public void SelectMonster()
    {
        isMonsterSelected = true;
    }

    void SpawnMonster()
    {
        // Evitar que el clic sobre la UI instancie el monstruo
        if (EventSystem.current.IsPointerOverGameObject()) return;

        // Obtener la posición del clic en el mundo
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit)) // Para juegos 3D
        {
            Instantiate(monsterSaved, hit.point + Vector3.up * 2, Quaternion.identity);
            isMonsterSelected = false; // Resetear para evitar más instancias sin nueva selección
        }
    }
}
