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
            Morir();
        }
    }

    private void Morir()
    {
        Debug.Log("Jugador muerto - EL JUEGO SE DETIENE");

        // Detiene todo el juego
        Time.timeScale = 0f;
    }
}
