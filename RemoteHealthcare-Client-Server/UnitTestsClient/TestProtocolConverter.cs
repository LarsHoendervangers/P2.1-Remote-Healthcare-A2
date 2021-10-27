using Microsoft.VisualStudio.TestTools.UnitTesting;
using RemoteHealthcare_Client.Ergometer.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestsClient
{
    [TestClass]
    public class TestProtocolConverter
    {
        [TestMethod]
        public void Test_ByteArrayToString_Success()
        {
            // Arrange
            byte[] bytesToTest = new byte[] { 14, 255, 0, 67, 90 };
            string expected = "1425506790";

            // Act
            string result = ProtocolConverter.ByteArrayToString(bytesToTest);

            // Assert
            Assert.AreEqual(expected, result);
        }
        
        [TestMethod]
        public void Test_ByteArrayToString_EmptyArray()
        {
            // Arrange
            byte[] bytesToTest = new byte[] {};
            string expected = "";

            // Act
            string result = ProtocolConverter.ByteArrayToString(bytesToTest);

            // Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Test_PageChecker_Success()
        {
            // Arrange
            byte[] bytesToTest = new byte[] { 14, 255, 0, 67, 90 };
            byte expected = 14;

            // Act
            byte result = ProtocolConverter.PageChecker(bytesToTest);

            // Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Test_ReadDataSet_WithCombine_Success()
        {
            // Arrange
            byte[] bytesToTest = new byte[] { 14, 255, 0, 67, 90 };
            byte targetnumber = 14;
            int targetIndex = 4;
            int expected = 90;

            // Act
            int result = ProtocolConverter.ReadDataSet(bytesToTest, targetnumber, true, targetIndex);

            // Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Test_ReadDataSet_WithoutCombine_Success()
        {
            // Arrange
            byte[] bytesToTest = new byte[] { 14, 255, 0, 67, 90 };
            byte targetnumber = 14;
            int targetIndex = 4;
            int expected = 90;

            // Act
            int result = ProtocolConverter.ReadDataSet(bytesToTest, targetnumber, false, targetIndex);

            // Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Test_ReadByte_Success()
        {
            // Arrange
            byte[] bytesToTest = new byte[] { 14, 255, 0, 67, 90 };
            int targetIndex = 4;
            int expected = 90;

            // Act
            int result = ProtocolConverter.ReadByte(bytesToTest, targetIndex);

            // Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Test_ReadByte_OutOfBounds()
        {
            // Arrange
            byte[] bytesToTest = new byte[] { 14, 255, 0, 67, 90 };
            int targetIndex = 5;
            int expected = -1;

            // Act
            int result = ProtocolConverter.ReadByte(bytesToTest, targetIndex);

            // Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Test_DataToPayload_Success()
        {
            // Arrange
            byte[] bytesToTest = new byte[] { 0b1111, 0b1010, 0b1111, 0b1010, 0b1011, 0b0001, 0b0110, 0b0101, 0b0001, 0b1010, 0b0010, 0b0011, 0b0100 };
            byte[] expected = new byte[] { 0b1011, 0b0001, 0b0110, 0b0101, 0b0001, 0b1010, 0b0010, 0b0011 };

            // Act
            byte[] result = ProtocolConverter.DataToPayload(bytesToTest);

            // Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Test_DataToPayload_Fail()
        {
            // Arrange
            byte[] bytesToTest = new byte[] { 0b1111, 0b1010, 0b1111, 0b1010, 0b1011, 0b0001, 0b0110, 0b0101, 0b0001, 0b1010, 0b0010 };
            byte[] expected = new byte[8];

            // Act
            byte[] result = ProtocolConverter.DataToPayload(bytesToTest);

            // Assert
            Assert.AreEqual(expected, result);
        }
    }
}
