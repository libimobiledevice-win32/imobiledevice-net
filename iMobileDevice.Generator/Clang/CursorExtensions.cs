using Core.Clang;
using System.Reflection;

namespace iMobileDevice.Generator.Clang
{
    public static class CursorExtensions
    {
        /// <summary>
        ///     Given a cursor that represents a documentable entity (e.g., declaration), return the
        ///     associated parsed comment as a full-comment AST node.
        /// </summary>
        /// <returns>The parsed comment.</returns>
        public static Comment GetParsedComment(this Cursor cursor) => Clang.CursorGetParsedComment(GetCXCursor(cursor));

        static readonly PropertyInfo structProperty = typeof(Cursor).GetProperty("Struct", BindingFlags.NonPublic | BindingFlags.Instance);
        static readonly FieldInfo kindField = structProperty.PropertyType.GetField("kind");
        static readonly FieldInfo xdataField = structProperty.PropertyType.GetField("xdata");
        static readonly FieldInfo data0Field = structProperty.PropertyType.GetField("data_0");
        static readonly FieldInfo data1Field = structProperty.PropertyType.GetField("data_1");
        static readonly FieldInfo data2Field = structProperty.PropertyType.GetField("data_2");

        private static CXCursor GetCXCursor(this Cursor cursor)
        {
            var value = structProperty.GetValue(cursor);

            return new CXCursor()
            {
                kind = (int)value.GetType().GetField("kind").GetValue(value),
                xdata = (int)value.GetType().GetField("xdata").GetValue(value),
                data_0 = (ulong)value.GetType().GetField("data_0").GetValue(value),
                data_1 = (ulong)value.GetType().GetField("data_1").GetValue(value),
                data_2 = (ulong)value.GetType().GetField("data_2").GetValue(value),
            };
        }
    }
}
