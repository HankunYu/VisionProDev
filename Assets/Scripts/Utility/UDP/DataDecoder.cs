using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace hankun.udp
{
    public class DataDecoder : MonoBehaviour
    {
        public static T Decode<T>(string data)
        {
            if (typeof(T) == typeof(bool))
            {
                var result = bool.TryParse(data, out var value) && value;
                return (T) Convert.ChangeType(result, typeof(bool));
            }

            if (typeof(T) == typeof(List<Quaternion>))
            {
                var result = new List<Quaternion>();
                var quaternions = data.Split(',');
                foreach (var quaternion in quaternions)
                {
                    if(quaternion == "") continue;
                    var values = quaternion.Split('|');
                    var x = float.Parse(values[0]);
                    var y = float.Parse(values[1]);
                    var z = float.Parse(values[2]);
                    var w = float.Parse(values[3]);
                    result.Add(new Quaternion(x, y, z, w));
                }

                return (T) Convert.ChangeType(result, typeof(List<Quaternion>));
            }
            return default;
        }
    
        public static string CoderQuaternion(List<Quaternion> data)
        {
            var result = "";
            foreach (var quaternion in data)
            {
                result += quaternion.x + "|" + quaternion.y + "|" + quaternion.z + "|" + quaternion.w + ",";
            }
            return result;
        }
    }
}
