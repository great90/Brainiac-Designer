////////////////////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2009, Daniel Kollmann
// All rights reserved.
//
// Redistribution and use in source and binary forms, with or without modification, are permitted
// provided that the following conditions are met:
//
// - Redistributions of source code must retain the above copyright notice, this list of conditions
//   and the following disclaimer.
//
// - Redistributions in binary form must reproduce the above copyright notice, this list of
//   conditions and the following disclaimer in the documentation and/or other materials provided
//   with the distribution.
//
// - Neither the name of Daniel Kollmann nor the names of its contributors may be used to endorse
//   or promote products derived from this software without specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR
// IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR
// CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
// DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
// DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY,
// WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY
// WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
////////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;
using Brainiac.Design.FileManagers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Brainiac.Test
{
	[TestClass]
	public class TestFileMethods
	{
		[TestMethod]
		public void TestMakeRelative()
		{
			string relativeTo= "c:\\test folder\\";

			string makeRelative= "filename.txt";
			string result= FileManager.MakeRelative(relativeTo, makeRelative);
			Assert.IsTrue(result =="filename.txt");

			makeRelative= "c:\\Test Folder\\filename.txt";
			result= FileManager.MakeRelative(relativeTo, makeRelative);
			Assert.IsTrue(result =="filename.txt");

			makeRelative= "d:\\filename.txt";
			result= FileManager.MakeRelative(relativeTo, makeRelative);
			Assert.IsTrue(result =="d:\\filename.txt");

			makeRelative= "c:\\filename.txt";
			result= FileManager.MakeRelative(relativeTo, makeRelative);
			Assert.IsTrue(result =="..\\filename.txt");

			makeRelative= "c:\\another folder\\filename.txt";
			result= FileManager.MakeRelative(relativeTo, makeRelative);
			Assert.IsTrue(result =="..\\another folder\\filename.txt");
		}

		[TestMethod]
		public void TestMakeAbsolute()
		{
			string absolutePath= "c:\\test folder\\sub folder\\";

			string relativePath= "hallo.txt";
			string result= FileManager.MakeAbsolute(absolutePath, relativePath);
			Assert.IsTrue(result ==absolutePath + relativePath);

			relativePath= "test\\hallo.txt";
			result= FileManager.MakeAbsolute(absolutePath, relativePath);
			Assert.IsTrue(result ==absolutePath + relativePath);

			relativePath= "..\\hallo.txt";
			result= FileManager.MakeAbsolute(absolutePath, relativePath);
			Assert.IsTrue(result =="c:\\test folder\\hallo.txt");

			try
			{
				relativePath= "..\\..\\..\\..\\hallo.txt";
				FileManager.MakeAbsolute(absolutePath, relativePath);
				Assert.IsTrue(false);
			}
			catch
			{
				Assert.IsTrue(true);
			}
		}
	}
}
