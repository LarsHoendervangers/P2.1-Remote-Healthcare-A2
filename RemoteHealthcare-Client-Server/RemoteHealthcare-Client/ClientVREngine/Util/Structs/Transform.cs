namespace RemoteHealthcare.ClientVREngine.Util.Structs
{

    /// <summary>
    /// Struct that stors transform data of a object in a scene
    /// </summary>
    public struct Transform
    {
        //Attributes NOT capitalized, to correstpond with server commands
        public double[] position { get; set; }
        public double scale { get; set; }
        public double[] rotation { get; set; }
        public Transform(double scale, double[] pos, double[] rot)
        {
            rotation = rot;
            this.scale = scale;
            position = pos;
        }
    }
}
