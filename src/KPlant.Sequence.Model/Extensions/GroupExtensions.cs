namespace KPlant.Sequence.Model
{
    public static class GroupExtensions
    {
        public static Group WithElse(this Group group, params Group[] @else)
        {
            group.Else.AddRange(@else);
            return group;
        }
    }
}