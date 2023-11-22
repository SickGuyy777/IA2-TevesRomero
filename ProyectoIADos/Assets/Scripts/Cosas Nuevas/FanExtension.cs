using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public static class FanExtension
{
    private static System.Random random = new System.Random();
    private static string[] abucheos = { "¡Animos!", "¡Vamos Atrapalo!", "¡No Dejes Que Te Gane!" };

    public static string GenerarAbucheo(this object obj)
    {
        int indiceAleatorio = random.Next(abucheos.Length);
        return abucheos[indiceAleatorio];
    }


    public static IEnumerable<Recompensa> GenerarRecompensaGanador(this IEnumerable<Agent> ganadores)
    {
        var ganador = ganadores.OrderByDescending(agente => agente._Time).FirstOrDefault();//IA2-LINQ 
        if (ganador != null && ganador.yo != null)
        {
            ActivarRecompensa(ganador.yo.position);
            yield return new Recompensa
            {
                Descripcion = $"¡Felicidades, {ganador.agentName}! Recibes una mejora de velocidad temporal.",
                ObjetoRecompensa = null 
            };
        }
    }
    private static void ActivarRecompensa(Vector3 posicionGanador)
    {
        GameObject recompensaPrefab = GameObject.Find("RecompensaPrefab");

        if (recompensaPrefab != null)
        {
            Vector3 nuevaPosicion = new Vector3(posicionGanador.x, 1.5f, posicionGanador.z);
            recompensaPrefab.transform.position = nuevaPosicion;
            recompensaPrefab.SetActive(true);
        }
        else
        {
            Debug.LogError("No se pudo encontrar el prefab de la recompensa.");
        }
    }
}
