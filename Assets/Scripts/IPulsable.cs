// IPulsable.cs
public interface IPulsable
{
    // intensity: 0..1 (0 = nenhum pulso, 1 = pulso máximo)
    void Pulse(float intensity);
}