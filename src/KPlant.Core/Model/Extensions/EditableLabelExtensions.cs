namespace KPlant.Model
{
    public static class EditableLabelExtensions
    {
        public static T Labelled<T>(this T instance, string label)
            where T: IEditableLabel
        {
            instance.Label = label;
            return instance;
        }
    }
}
