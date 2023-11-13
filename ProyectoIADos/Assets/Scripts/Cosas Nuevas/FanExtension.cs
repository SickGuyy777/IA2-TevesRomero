using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FanExtension
{
    private static System.Random random = new System.Random();
    private static string[] abucheos = { "¡Animos!", "¡Vamos Atrapalo!", "¡No Dejes Que Te Gane!" };

    public static string GenerarAbucheo(this object obj)
    {
        int indiceAleatorio = random.Next(abucheos.Length);
        return abucheos[indiceAleatorio];
    }

}
