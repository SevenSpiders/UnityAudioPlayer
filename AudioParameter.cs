
[System.Serializable]
public struct AudioParameters {
    public float pitch;
    public float volume;

    public AudioParameters(float volume, float pitch) {
        this.volume = volume;
        this.pitch = pitch;
    }

    public static AudioParameters one => new AudioParameters(1f, 1f);
}
