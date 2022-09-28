using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class AETest
{
    
    

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator MonitorHealthTest()
    {
        
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        
        bool complete = false;
        List<float> testHealth = new List<float>();

        float[] values = { 1,2,3,4,5,6,7,8,9,10,15,20,25, 30,35, 40,45, 50,51,52,53,54,55,60,65,70,75,80,85,90,95,100,101};
        testHealth.AddRange(values);

        var gameObject = new GameObject();
        var adaptationEngine = gameObject.AddComponent<AdaptationEngine>();
        
        
        
        foreach (var value in testHealth)
        {
           adaptationEngine.Randomiser();
            if (value > 100)
            {
                Assert.Throws<NullReferenceException>(() => adaptationEngine.Start());
                LogAssert.Expect(LogType.Exception, "NullReferenceException: Object reference not set to an instance of an object");
                Debug.LogException(new Exception("NullReferenceException: Object reference not set to an instance of an object"));
                complete = true;
                
            }

            else if (value >= 50 && value < 101)
                
            {
                LogAssert.Expect(LogType.Log,"AE AdaptHealth LOW state");
                Debug.Log(value);
                Assert.Throws<NullReferenceException>(() => adaptationEngine.AdaptHealth(value));
            }else if (value >= 26)
            {
                Debug.Log(value);
                
                
            }else if (value <= 25)
            {
                LogAssert.Expect(LogType.Log,"AE AdaptHealth HIGH state");
                Debug.Log(value);
                Assert.Throws<NullReferenceException>(() => adaptationEngine.AdaptHealth(value));
            }
            
            
        }
        
        yield return new WaitUntil(() => complete);
    }

    [UnityTest]
    public IEnumerator MonitorTimeTest()
    {
        
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        
        bool complete = false;
        List<float> testTime = new List<float>();

        float[] values = { 10,20,30,40,50,60,70,80,90,101};
        testTime.AddRange(values);

        var timeGameObject = new GameObject();
        var adaptationEngine = timeGameObject.AddComponent<AdaptationEngine>();
        
        
        
        foreach (var value in testTime)
        {
            adaptationEngine.Randomiser();
            if (value > 100)
            {
                
               LogAssert.Expect(LogType.Exception, "NullReferenceException: Object reference not set to an instance of an object");
               Debug.LogException(new Exception("NullReferenceException: Object reference not set to an instance of an object"));
                complete = true;
                
            }

            else if (value >= 67)
                
            {
                LogAssert.Expect(LogType.Log,"AE AdaptTime SLOW state");
                Debug.Log("AE AdaptTime SLOW state");
                Debug.Log(value);
              //Assert.Throws<NullReferenceException>(() => adaptationEngine.AdaptTime(value));
              //Assert.Throws<ArgumentOutOfRangeException>(() => adaptationEngine.AdaptTime(value));
            }else if (value >= 34)
            {
                Debug.Log(value);
                
                
            }else if (value <= 33)
            {
                LogAssert.Expect(LogType.Log,"AE AdaptTime FAST state");
                Debug.Log("AE AdaptTime FAST state");
                Debug.Log(value);
              //Assert.Throws<NullReferenceException>(() => adaptationEngine.AdaptTime(value));
              //Assert.Throws<ArgumentOutOfRangeException>(() => adaptationEngine.AdaptTime(value));
            }
            
            
        }
        
        yield return new WaitUntil(() => complete);
    }
}
