using KPlant.Tests.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using System.Collections;

namespace KPlant.Sequence.Model.UnitTests
{
    public class ArrowHeadTests
    {
        [Fact]
        public void Ctor_SetsDefaults()
        {
            var sut = new ArrowHead();
            Assert.Equal(ArrowHeadThickness.Normal, sut.Thickness);
            Assert.Equal(ArrowHeadParts.Normal, sut.Parts);
            Assert.Equal(ArrowHeadStatus.Normal, sut.Status);
        }
        
        [Fact]
        public void Render_Defaults_Renders()
        {
            var sut = new ArrowHead();

            var result = sut.Render();

            Assert.Equal(">", result);
        }

        [Theory]        
        [InlineData(ArrowHeadParts.Normal, ">")]
        [InlineData(ArrowHeadParts.Top, @"\")]
        [InlineData(ArrowHeadParts.Bottom, "/")]
        public void Render_Parts_Renders(ArrowHeadParts parts, string expected)
        {
            var sut = new ArrowHead { Parts = parts };

            var result = sut.Render();

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(ArrowHeadParts.Normal, ">>")]
        [InlineData(ArrowHeadParts.Top, @"\\")]
        [InlineData(ArrowHeadParts.Bottom, "//")]
        public void Render_ThinParts_Renders(ArrowHeadParts parts, string expected)
        {
            var sut = new ArrowHead { Parts = parts, Thickness = ArrowHeadThickness.Thin };

            var result = sut.Render();

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(ArrowHeadStatus.Success, ">o")]
        [InlineData(ArrowHeadStatus.Fail, ">x")]
        public void Render_Status_Renders(ArrowHeadStatus status, string expected)
        {
            var sut = new ArrowHead { Status = status };

            var result = sut.Render();

            Assert.Equal(expected, result);
        }
    }
}
