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
            byte[] bytesToTest = new byte[] { 15, 56, 1, 67, 90, 123, 34, 4, 87, 56, 99, 9, 3 };
            byte[] expected = new byte[] { 90, 123, 34, 4, 87, 56, 99, 9 };

            // Act
            byte[] result = ProtocolConverter.DataToPayload(bytesToTest);
            bool isTheSame = false;

            for (int i = 0; i < expected.Length; i++)
            {
                if (result[i] != expected[i])
                {
                    isTheSame = false;
                    break;
                }
                else
                {
                    isTheSame = true;
                }
            }

            // Assert
            Assert.IsTrue(isTheSame);
        }

        [TestMethod]
        public void Test_DataToPayload_Fail()
        {
            // Arrange
            byte[] bytesToTest = new byte[] { 0b1111, 0b1010, 0b1111, 0b1010, 0b1011, 0b0001, 0b0110, 0b0101, 0b0001, 0b1010, 0b0010 };
            byte[] expected = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0};

            // Act
            byte[] result = ProtocolConverter.DataToPayload(bytesToTest);
            bool isZero = false;
            
            foreach (byte b in result)
            {
                if (b != 0)
                {
                    isZero = false;
                    break;
                } else
                {
                    isZero = true;
                }
            }

            // Assert
            Assert.IsTrue(isZero);
        }

        [TestMethod]
        public void Test_CombineBits_Succes()
        {
            // Arrange
            byte byte1 = 0b10101010;
            byte byte2 = 0b01010101;
            ushort expected = 43605;

            // Act
            ushort result = ProtocolConverter.CombineBits(byte1, byte2);

            // Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Test_MichaelChecksum_Succes()
        {
            // Arrange
            byte[] bytes = new byte[] { 15, 56, 1, 67, 90, 123, 34, 4, 87, 56, 99, 9, 119 };
            
            // Act
            bool result = ProtocolConverter.MichaelChecksum(bytes);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Test_MichaelChecksum_Fail()
        {
            // Arrange
            byte[] bytes = new byte[] { 15, 56, 1, 67, 90, 123, 34, 4, 87, 56, 99, 9 };
            
            // Act
            bool result = ProtocolConverter.MichaelChecksum(bytes);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Test_RollOver_Success()
        {
            // Arrange
            int value = 513;
            int oldValue = 512;
            int counter = 2;
            int expected = counter * 256 + value;

            // Act
            int result = ProtocolConverter.rollOver(value, ref oldValue, ref counter);

            // Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Test_RollOverTotalPower_Success()
        {
            // Arrange
            int value = 65536 * 2;
            int oldValue = (65536 * 2) - 1;
            int counter = 2;
            int expected = counter * 65536 + value;

            // Act
            int result = ProtocolConverter.rollOverTotalPower(value, ref oldValue, ref counter);

            // Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Test_ConfirmPageData_Success()
        {
            // Arrange
            byte[] bytes = new byte[] { 0x16, 0x10, 0x00 };

            // Act
            bool result = ProtocolConverter.ConfirmPageData(bytes);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Test_ConfirmPageData_Fail()
        {
            // Arrange
            byte[] bytes = new byte[] { 0x15, 0x10, 0x00 };

            // Act
            bool result = ProtocolConverter.ConfirmPageData(bytes);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Test_TransformtoKMH_Success()
        {
            // Arrange
            double value = 15;
            double expected = 0.054;

            // Act
            double result = ProtocolConverter.TransformtoKMH(value);

            // Assert
            Assert.IsTrue(result == expected);
        }
    }
}
