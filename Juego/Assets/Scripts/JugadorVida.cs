using UnityEngine;

public class VidaJugador : MonoBehaviour
{
    public int vida = 100;

    public void RecibirDaño(int cantidad)
    {
        vida -= cantidad;
        Debug.Log("Vida actual: " + vida);

        if (vida <= 0)
        {
            Debug.Log("Jugador muerto");
        }
    }
}
