public interface ISpell
{
    int ManaCost { get; } // Свойство для стоимости маны
    void Activate();      // Метод активации спелла
}