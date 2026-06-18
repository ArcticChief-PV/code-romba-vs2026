using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using WinterbiteStudios.CodeRomba.Model.Comments;
using WinterbiteStudios.CodeRomba.Model.Comments.Options;

namespace WinterbiteStudios.CodeRomba.UnitTests.Formatting
{
    internal class CommentFormatHelper
    {
        public static string AssertEqualAfterFormat(
             string text,
             Action<FormatterOptions> options = null)
        {
            return AssertEqualAfterFormat(text, null, null, options);
        }

        public static string AssertEqualAfterFormat(
                  string text,
            string expected,
                  Action<FormatterOptions> options = null)
        {
            return AssertEqualAfterFormat(text, expected, null, options);
        }

        public static string AssertEqualAfterFormat(
            string text,
            string expected,
            string prefix,
            Action<FormatterOptions> options = null)
        {
            var result = CodeComment.Format(text, prefix, options);
            Assert.AreEqual(expected ?? text, result);
            return result;
        }
    }
}