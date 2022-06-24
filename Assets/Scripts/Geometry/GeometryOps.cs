using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GeometryOps
{
    private const float threshold = 0.001f;

    #region Edge

    public static Vector3 ProjectOnEdge(ref Vector3 p, ref Vector3 src, ref Vector3 dst)
    {
        var srcToDst = dst - src;
        var srcToP = p - src;
        float projMagnitude = Vector3.Dot(srcToP, srcToDst) / srcToDst.magnitude;
        //inside = dotVal >= 0 && dotVal <= srcToDst.magnitude;
        if (projMagnitude > srcToDst.magnitude)
        {
            return dst;
        }
        else if (projMagnitude < 0)
        {
            return src;
        }
        else
        {
            var proj = src + srcToDst.normalized * projMagnitude;
            return proj;
        }
    }

    #endregion

    #region Triangle

    public static Vector3 ProjectOnTriangle(ref Vector3 p, ref Vector3 a, ref Vector3 b, ref Vector3 c, out bool wasOutside)
    {
        Vector3 ab = b - a;
        Vector3 ac = c - a;
        var proj = ProjectOnPlane(ref p, ref a, ref ab, ref ac);
        proj = ClampPointInsideTriangle(ref proj, ref a, ref b, ref c, out wasOutside);
        return proj;
    }

    public static Vector3 ProjectLineOnTriangle(ref Vector3 p, ref Vector3 lineDir, ref Vector3 a, ref Vector3 b, ref Vector3 c, out bool wasOutside)
    {
        Vector3 ab = b - a;
        Vector3 ac = c - a;
        var proj = ProjectLineOnPlane(ref p, ref lineDir, ref a, ref ab, ref ac);
        proj = ClampPointInsideTriangle(ref proj, ref a, ref b, ref c, out wasOutside);
        return proj;
    }

    private static Vector3 ClampPointInsideTriangle(ref Vector3 p, ref Vector3 a, ref Vector3 b, ref Vector3 c, out bool wasOutside)
    {
        if (!PointInsideTriangle(ref p, ref a, ref b, ref c))
        {
            wasOutside = true;
            Vector3[] projsOnEdges = new Vector3[]
            {
                ProjectOnEdge(ref p, ref a, ref b),
                ProjectOnEdge(ref p, ref a, ref c),
                ProjectOnEdge(ref p, ref b, ref c),
            };

            float minDist = float.MaxValue;
            int idx = 0;
            for (int i = 0; i < projsOnEdges.Length; i++)
            {
                float dist = Vector3.Distance(p, projsOnEdges[i]);
                if (dist < minDist)
                {
                    minDist = dist;
                    idx = i;
                }
            }
            p = projsOnEdges[idx];
        }
        else
            wasOutside = false;

        return p;
    }

    public static Vector3 ProjectOnPlane(ref Vector3 p, ref Vector3 planeP, ref Vector3 planeNormal)
    {
        Vector3 proj = planeP + Vector3.ProjectOnPlane((p - planeP), planeNormal);
        return proj;
    }

    public static Vector3 ProjectOnPlane(ref Vector3 p, ref Vector3 planeP, ref Vector3 planeDir0, ref Vector3 planeDir1)
    {
        Vector3 planeNormal = Vector3.Cross(planeDir0, planeDir1).normalized;
        return ProjectOnPlane(ref p, ref planeP, ref planeNormal);
    }

    public static Vector3 ProjectLineOnPlane(ref Vector3 p, ref Vector3 lineDir, ref Vector3 planeP, ref Vector3 planeDir0, ref Vector3 planeDir1)
    {
        Vector3 proj = ProjectOnPlane(ref p, ref planeP, ref planeDir0, ref planeDir1);
        Vector3 dir = Vector3.Dot((proj - p).normalized, lineDir.normalized) >= 0 ? lineDir : -lineDir;
        float alfa = Vector3.Angle((proj - p).normalized, dir.normalized);
        float hypotenuse = (proj - p).magnitude / Mathf.Cos(alfa * Mathf.Deg2Rad);
        //Vector3 dir = (Vector3.Dot(proj - p, lineDir) >= 0 ? lineDir : -lineDir).normalized;
        Vector3 intersection = p + dir * hypotenuse;
        return intersection;
    }

    /*
     * Checks if a point belongs to a triangle. The point must belong to the plane formed by the triangle edges.
     * If the point is not projected, it should be projected first
     */
    public static bool PointInsideTriangle(ref Vector3 p, ref Vector3 a, ref Vector3 b, ref Vector3 c)
    {
        /*float ABC = TriangleArea(ref a, ref b, ref c);
        float PAB = TriangleArea(ref p, ref a, ref b);
        float PBC = TriangleArea(ref p, ref b, ref c);
        float PAC = TriangleArea(ref p, ref a, ref c);
        bool inside = Mathf.Abs(ABC - (PAB + PBC + PAC)) < threshold;*/
        (float w1, float w2, float w3) = GetLerpWeightsForPointInTriangle(ref p, ref a, ref b, ref c);
        bool inside = w1 >= 0 && w2 >= 0 && w3 >= 0;
        return inside;
    }

    public static float TriangleArea(ref Vector3 a, ref Vector3 b, ref Vector3 c)
    {
        return Vector3.Cross((a - b), (a - c)).magnitude / 2f;
    }

    public static Vector3 TriangleNormal(ref Vector3 a, ref Vector3 b, ref Vector3 c)
    {
        return -Vector3.Cross((b - a), (b - c)).normalized;
    }

    // https://answers.unity.com/questions/383804/calculate-uv-coordinates-of-3d-point-on-plane-of-m.html
    public static (float w0, float w1, float w2) GetLerpWeightsForPointInTriangle(
        ref Vector3 p, ref Vector3 t1, ref Vector3 t2, ref Vector3 t3)
    {
        var f1 = t1 - p;
        var f2 = t2 - p;
        var f3 = t3 - p;

        Vector3 va= Vector3.Cross(t1 - t2, t1 - t3); // main triangle cross product
        Vector3 va1= Vector3.Cross(f2, f3); // p1's triangle cross product
        Vector3 va2= Vector3.Cross(f3, f1); // p2's triangle cross product
        Vector3 va3= Vector3.Cross(f1, f2); // p3's triangle cross product
        float a = va.magnitude; // main triangle area
        
        // calculate barycentric coordinates with sign:
        float a1 = va1.magnitude / a * Mathf.Sign(Vector3.Dot(va, va1));
        float a2 = va2.magnitude / a * Mathf.Sign(Vector3.Dot(va, va2));
        float a3 = va3.magnitude / a * Mathf.Sign(Vector3.Dot(va, va3));

        return (a1, a2, a3);
    }

    public static Vector3 TransformPointBetweenTriangles(Vector3 p,
        ref Vector3 a1, ref Vector3 b1, ref Vector3 c1,
        ref Vector3 a2, ref Vector3 b2, ref Vector3 c2)
    {
        (float w1, float w2, float w3) = GetLerpWeightsForPointInTriangle(ref p, ref a1, ref b1, ref c1);
        return a2 * w1 + b2 * w2 + c2 * w3;
    }

    //Por teorema del seno
    /*
    public static Vector3 TransformPointBetweenTriangles2(Vector3 p,
        ref Vector3 a1, ref Vector3 b1, ref Vector3 c1,
        ref Vector3 a2, ref Vector3 b2, ref Vector3 c2)
    {
        p = ProjectOnTriangle(ref p, ref a1, ref b1, ref c1);

        float alfa = Vector3.Angle(c1 - a1, b1 - a1);
        float alfaB = Vector3.Angle(p - a1, b1 - a1);

        float beta = Vector3.Angle(a1 - b1, c1 - b1);
        float betaA = Vector3.Angle(a1 - b1, p - b1);

        Vector3 finalP;

        if (alfaB == 0 || betaA == 0)
        {
            float pct = (p - b1).magnitude / (a1-b1).magnitude;
            finalP = b2 + (a2 - b2) * pct;
        }
        else
        {
            float alfaBpct = alfaB / alfa;
            float betaApct = betaA / beta;

            float alfaPrim = Vector3.Angle(c2 - a2, b2 - a2);
            float betaPrim = Vector3.Angle(a2 - b2, c2 - b2);

            float alfaBPrim = alfaBpct * alfaPrim;
            float betaAPrim = betaApct * betaPrim;

            float alfaBPrimRad = alfaBPrim * Mathf.Deg2Rad;
            float betaAPrimRad = betaAPrim * Mathf.Deg2Rad;

            //Vector3 apPrim = (Quaternion.AngleAxis(alfaBPrim, Vector3.Cross(b2 - a2, c2 - a2).normalized) * (b2 - a2)).normalized;
            Vector3 bpPrim = (Quaternion.AngleAxis(betaAPrim, Vector3.Cross(a2 - b2, c2 - b2).normalized) * (a2 - b2)).normalized;

            float h = Mathf.Sin(alfaBPrimRad) * (a2 - b2).magnitude;
            float beta1 = betaAPrimRad - (90 * Mathf.Deg2Rad - alfaBPrimRad);
            float hypotenuse = h / Mathf.Cos(beta1);
            finalP = b2 + bpPrim * hypotenuse;
        }
        
        return finalP;
    }*/

    #endregion
}
