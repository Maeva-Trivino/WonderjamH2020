namespace Interactive.Base
{
    public interface Interactable
    {
        // Sélectionne l'objet (mettre en surbrillance)
        void Select();

        // Désélectionne l'objet
        void Deselect();

        // Renvoyer une description d'action
        string GetDecription(Player contextPlayer);
    }
}