using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QTE;

public interface Interactive
{
    // Sélectionne l'objet (mettre en surbrillance)
    void Select();

    // Désélectionne l'objet
    void Deselect();

    UserAction GetAction(Player player);
}