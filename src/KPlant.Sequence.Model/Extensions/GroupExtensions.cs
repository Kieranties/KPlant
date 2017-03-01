namespace KPlant.Sequence.Model
{
    public static class GroupExtensions
    {
        public static Group Else(this Group group, params Group[] @else)
        {
            group.Else.AddRange(@else);
            return group;
        }
    }
}