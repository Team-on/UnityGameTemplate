#region License
//Copyright(c) 2017 Akase Matsuura
//https://github.com/LightGive/SystemCalc

//Permission is hereby granted, free of charge, to any person obtaining a
//copy of this software and associated documentation files (the
//"Software"), to deal in the Software without restriction, including
//without limitation the rights to use, copy, modify, merge, publish, 
//distribute, sublicense, and/or sell copies of the Software, and to
//permit persons to whom the Software is furnished to do so, subject to
//the following conditions:

//The above copyright notice and this permission notice shall be
//included in all copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, 
//EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
//MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
//NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
//LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
//OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
//WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
#endregion
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 様々な計算を扱うクラス
/// </summary>
public static class SystemCalc
{
	/// <summary>
	/// 通常の重力加速度
	/// </summary>
	private const float DefaultGravitationalAcceleration = -9.80665f;
	/// <summary>
	/// 通常の空気抵抗
	/// </summary>
	private const float DefaultDrag = 0.0f;
	/// <summary>
	/// 通常の質量
	/// </summary>
	private const float DefaultMass = 1.0f;
	/// <summary>
	/// ネイピア数,2.718281828...
	/// </summary>
	private const float E = (float)System.Math.E;
	/// <summary>
	/// 通常の重力加速度のベクトル
	/// </summary>
	private static readonly Vector3 DefaultGravitationalAccelerationVec = new Vector3(0.0f, DefaultGravitationalAcceleration, 0.0f);

	#region 物理など

	#region GetVelocityTopTime (初速を加えた時、何秒後に頂点に達するかを求める)

	/// <summary>
	/// 初速を加えた時、何秒後に頂点に達するかを求める
	/// </summary>
	/// <param name="_vec">初速</param>
	/// <returns>何秒後に頂点に到達するか</returns>
	public static float GetVelocityTopTime(float _vec)
	{
		return GetVelocityTopTime(_vec, DefaultGravitationalAccelerationVec);
	}

	/// <summary>
	/// 初速を加えた時、何秒後に頂点に達するかを求める
	/// </summary>
	/// <param name="_vec">初速</param>
	/// <param name="_gravity">重力加速度</param>
	/// <returns>何秒後に頂点に到達するか</returns>
	public static float GetVelocityTopTime(Vector3 _vec, Vector3 _gravity)
	{
		return GetVelocityTopTime(_vec.y, _gravity);
	}

	/// <summary>
	/// 初速を加えた時、何秒後に頂点に達するかを求める
	/// </summary>
	/// <returns>頂点に到達する時間</returns>
	/// <param name="_vec">初速</param>
	/// <param name="_gravity">重力加速度</param>
	public static float GetVelocityTopTime(Vector3 _vec, float _gravity = DefaultGravitationalAcceleration)
	{
		return GetVelocityTopTime(_vec.y, new Vector3(0.0f, _gravity, 0.0f));
	}

	/// <summary>
	/// 初速を加えた時、何秒後に頂点に達するかを求める
	/// </summary>
	/// <returns>頂点に到達する時間</returns>
	/// <param name="_vec">初速</param>
	/// <param name="_gravity">重力加速度</param>
	public static float GetVelocityTopTime(Vector2 _vec, float _gravity = DefaultGravitationalAcceleration)
	{
		return GetVelocityTopTime(_vec.y, new Vector3(0.0f, _gravity, 0.0f));
	}

	/// <summary>
	/// 初速を加えた時、何秒後に頂点に達するかを求める
	/// </summary>
	/// <param name="_vec">初速</param>
	/// <param name="_gravity">重力加速度</param>
	/// <returns>何秒後に頂点に到達するか</returns>
	public static float GetVelocityTopTime(float _vec, Vector3 _gravity)
	{
		return Mathf.Clamp(_vec / -_gravity.y, 0.0f, float.PositiveInfinity);
	}

	#endregion

	#region GetVelocityTopPos (初速を加えた時の最高地点の座標を求める)

	/// <summary>
	/// 初速を加えた時の最高地点の座標を求める
	/// </summary>
	/// <param name="_vec">初速</param>
	/// <param name="_startPos">初速を加えた時の高さ</param>
	/// <returns>最高地点の座標</returns>
	public static Vector3 GetVelocityTopPos(Vector3 _vec, Vector3 _startPos)
	{
		return GetVelocityTopPos(_vec, _startPos, DefaultGravitationalAccelerationVec);
	}

	/// <summary>
	/// 初速を加えた時の最高地点の座標を求める
	/// </summary>
	/// <param name="_vec">初速</param>
	/// <param name="_startPos">初速を加えた時の高さ</param>
	/// <param name="_gravity">重力加速度</param>
	/// <returns></returns>
	public static Vector3 GetVelocityToPos(Vector3 _vec, Vector3 _startPos, float _gravity = DefaultGravitationalAcceleration)
	{
		return GetVelocityTopPos(_vec, _startPos, new Vector3(0.0f, _gravity, 0.0f));
	}

	/// <summary>
	/// 初速を加えた時の最高地点の座標を求める
	/// </summary>
	/// <param name="_vec">初速</param>
	/// <param name="_startPos">初速を加えた時の高さ</param>
	/// <param name="_gravity">重力加速度</param>
	/// <param name="_mass">質量</param>
	/// <param name="_drag">空気抵抗</param>
	/// <returns>最高地点の座標</returns>
	public static Vector3 GetVelocityTopPos(Vector3 _vec, Vector3 _startPos, Vector3 _gravity, float _mass = DefaultMass, float _drag = DefaultDrag)
	{
		//GetVelocityTopTime
		var t = Mathf.Clamp(_vec.y / -_gravity.y, 0.0f, float.PositiveInfinity);
		var h = (_vec.y * t) + (-0.5f * -_gravity.y * Mathf.Pow(t, 2.0f));
		return new Vector3(_vec.x * t, h, _vec.z * t) + _startPos;
	}


	#endregion

	#region GetVelocityTimeToPosition (初速を加えた後指定秒数後にどの座標にいるかを求める)

	/// <summary>
	/// 初速を加えた後指定秒数後にどの座標にいるか
	/// </summary>
	/// <param name="_vec">初速</param>
	/// <param name="_startPos">初速を加えた時の座標</param>
	/// <param name="_time">指定秒数</param>
	/// <returns></returns>
	public static Vector3 GetVelocityTimeToPosition(Vector3 _vec, Vector3 _startPos, float _time)
	{
		return GetVelocityTimeToPosition(_vec, _startPos, _time, DefaultGravitationalAccelerationVec);
	}

	/// <summary>
	/// 初速を加えた後指定秒数後にどの座標にいるか
	/// </summary>
	/// <returns>指定秒数後の座標</returns>
	/// <param name="_vec">初速</param>
	/// <param name="_startPos">初速を加えた時の座標</param>
	/// <param name="_time">指定秒数</param>
	/// <param name="_gravity">重力加速度</param>
	public static Vector3 GetVelocityTimeToPosition(Vector3 _vec, Vector3 _startPos, float _time, Vector3 _gravity)
	{
		return new Vector3(
			_vec.x * _time,
			(_vec.y * _time) - 0.5f * (-_gravity.y) * Mathf.Pow(_time, 2.0f),
			_vec.z * _time) + _startPos;
	}

	#endregion

	#region GetBallisticpredictionPoint (初速を加えた後の弾道予測点を求める)

	/// <summary>
	/// 初速を加えた後の弾道予測点を求める
	/// </summary>
	/// <returns>The ballisticprediction point.</returns>
	/// <param name="_vec">初速</param>
	/// <param name="_startPos">初速を加えた時の座標</param>
	/// <param name="_pointNum">取得する座標の数</param>
	/// <param name="_intervalTime">秒数ごとの間隔</param>
	/// <param name="_gravity">重力加速度</param>
	public static Vector3[] GetBallisticpredictionPoint(Vector3 _vec, Vector3 _startPos, int _pointNum, float _intervalTime, float _gravity = DefaultGravitationalAcceleration)
	{
		return GetBallisticpredictionPoint(_vec, _startPos, _pointNum, _intervalTime, new Vector3(0.0f, _gravity, 0.0f));
	}

	/// <summary>
	/// 初速を加えた後の弾道予測点を求める
	/// </summary>
	/// <returns>The ballisticprediction point.</returns>
	/// <param name="_vec">初速</param>
	/// <param name="_startPos">初速を加えた時の座標</param>
	/// <param name="_pointNum">取得する座標の数</param>
	/// <param name="_intervalTime">秒数ごとの間隔</param>
	/// <param name="_gravity">重力加速度</param>
	public static Vector3[] GetBallisticpredictionPoint(Vector3 _vec, Vector3 _startPos, int _pointNum, float _intervalTime, Vector3 _gravity)
	{
		_pointNum = Mathf.Clamp(_pointNum, 1, int.MaxValue);
		_intervalTime = Mathf.Clamp(_intervalTime, 0.0001f, float.MaxValue);

		Vector3[] points = new Vector3[_pointNum];
		for (int i = 0; i < _pointNum; i++)
		{
			points[i] = GetVelocityTimeToPosition(_vec, _startPos, (i * _intervalTime), _gravity);
		}
		return points;
	}
	#endregion

	#region GetVelocityGroundFallTimeVec (指定秒数後に指定の地面に落ちる初速を求める)

	public static bool GetVelocityGroundFallTimeVec(ref Vector3 _vec, Vector3 _startPos, Vector3 _targetPos, float _time, float _gravity = DefaultGravitationalAcceleration)
	{
		var offset = _targetPos.y - _startPos.y;
		var x = (_targetPos.x - _startPos.x) / _time;
		var z = (_targetPos.z - _startPos.z) / _time;
		var y = (_targetPos.y / _time) + (0.5f * _gravity * _time) - (_startPos.y / _time);
		_vec = new Vector3(x, -y, z);
		return true;
	}

	#endregion

	#region GetFreeFallTime (空気抵抗を含む指定距離の自由落下する時間を求める)

	public static float GetFreeFallTime(float _height, float _gravity, float _mass, float _drag)
	{
		return Mathf.Sqrt(_mass / (-_gravity * _drag)) * (float)System.Math.Acos((_height * _drag) / _mass);
	}

	#endregion

	#endregion

	#region 座標など

	#region GetLineNearPoint(ある座標の直線上の一番近い座標を求める)


	/// <summary>
	/// 二点間の線上で一番近い座標を求める
	/// </summary>
	/// <returns>線上で一番近い座標</returns>
	/// <param name="_linePoint1">線の始点</param>
	/// <param name="_linePoint2">線の終点</param>
	/// <param name="_checkPoint">確認する座標</param>
	/// <param name="_isLimit">0-1で制限をかけるかどうか<c>true</c> is limit.</param> 
	public static Vector3 GetLineNearPoint(Vector3 _linePoint1, Vector3 _linePoint2, Vector3 _checkPoint, bool _isLimit = false)
	{
		float lerp = 0.0f;
		return GetLineNearPoint(_linePoint1, _linePoint2, _checkPoint, _isLimit, out lerp);
	}

	/// <summary>
	/// 二点間の線上で一番近い座標を求める
	/// </summary>
	/// <returns>線上で一番近い座標</returns>
	/// <param name="_linePoint1">線の始点</param>
	/// <param name="_linePoint2">線の終点</param>
	/// <param name="_checkPoint">確認する座標</param>
	/// <param name="_isLimit">0-1で制限をかけるかどうか<c>true</c> is limit.</param>
	/// <param name="_lerp">線の始点から終点を0-1で確認できる</param>
	public static Vector3 GetLineNearPoint(Vector3 _linePoint1, Vector3 _linePoint2, Vector3 _checkPoint, bool _isLimit, out float _lerp)
	{
		var x = Vector3.Dot((_linePoint2 - _linePoint1).normalized, (_checkPoint - _linePoint1));
		if (_isLimit)
		{
			x = Mathf.Clamp(x, 0.0f, Vector3.Distance(_linePoint1, _linePoint2));
		}
		var nearPoint = _linePoint1 + (_linePoint2 - _linePoint1).normalized * x;
		_lerp = x / Vector3.Distance(_linePoint1, _linePoint2);
		return nearPoint;
	}

	#endregion

	#endregion

	#region 交点など

	/// <summary>
	/// 円と線との交点を求める
	/// </summary>
	/// <param name="_linePoint1">線のポイント１</param>
	/// <param name="_linePoint2">線のポイント２</param>
	/// <param name="_circleCenter">円の中心座標</param>
	/// <param name="_circleRadius">円の半径</param>
	/// <param name="_intersectionPoint1">交点１</param>
	/// <param name="_intersectionPoint2">交点２</param>
	/// <returns>円と点が接するか</returns>
	public static bool GetIntersectionOfLineAndCircle(Vector2 _linePoint1, Vector2 _linePoint2, Vector2 _circleCenter, float _circleRadius, out Vector2 _intersectionPoint1, out Vector2 _intersectionPoint2)
	{
		float float_1 = _linePoint2.y - _linePoint1.y;
		float float_2 = _linePoint1.x - _linePoint2.x;
		float float_3 = -((float_1 * _linePoint1.x) + (float_2 * _linePoint1.y));
		float float_4 = (float_1 * float_1) + (float_2 * float_2);
		float float_5 = float_1 * _circleCenter.x + float_2 * _circleCenter.y + float_3;
		float float_6 = float_4 * _circleRadius * _circleRadius - float_5 * float_5;

		_intersectionPoint1 = Vector2.zero;
		_intersectionPoint2 = Vector2.zero;
		Vector2 h = Vector2.zero;

		//二点が同じ位置の時や、二点とも円の中にある時は交差しない判定にする
		if ((_linePoint1 == _linePoint2) ||
			((Vector2.Distance(_circleCenter, _linePoint1) < _circleRadius && Vector2.Distance(_circleCenter, _linePoint2) < _circleRadius)))
			return false;

		if (float_6 > 0)
		{
			float float_7 = _circleCenter.x - (float_1 / float_4) * float_5;
			float float_8 = _circleCenter.y - (float_2 / float_4) * float_5;
			float float_9 = (float_2 / float_4) * Mathf.Sqrt(float_6);
			float float_10 = (float_1 / float_4) * Mathf.Sqrt(float_6);
			_intersectionPoint1 = new Vector2(float_7 - float_9, float_8 + float_10);
			_intersectionPoint2 = new Vector2(float_7 + float_9, float_8 - float_10);
			h = new Vector2((_linePoint1.x + _linePoint2.x) / 2.0f, (_linePoint1.y + _linePoint2.y) / 2.0f);

			if ((Vector2.Distance(_circleCenter, _linePoint1) < _circleRadius && Vector2.Distance(_circleCenter, _linePoint2) >= _circleRadius) || (Vector2.Distance(_circleCenter, _linePoint2) < _circleRadius && Vector2.Distance(_circleCenter, _linePoint1) >= _circleRadius))
			{
				if (Vector2.Distance(_intersectionPoint1, h) > Vector2.Distance(_intersectionPoint2, h))
				{ _intersectionPoint1 = Vector2.zero; }
				else
				{ _intersectionPoint2 = Vector2.zero; }
				return true;
			}
			else
			{
				var v = _linePoint2 - _linePoint1;
				var c = _circleCenter - _linePoint1;
				var n1 = Vector2.Dot(v, c);

				if (n1 < 0)
				{
					return c.magnitude < _circleRadius;
				}

				var n2 = Vector2.Dot(v, v);
				if (n1 > n2)
				{
					var len = Mathf.Pow(Vector2.Distance(_circleCenter, _linePoint2), 2);
					return (len < Mathf.Pow(_circleRadius, 2));
				}
				else
				{
					var n3 = Vector2.Dot(c, c);
					return (n3 - (n1 / n2) * n1 < Mathf.Pow(_circleRadius, 2));
				}
			}
		}
		else if (float_6.Equals(0.0f))
		{
			var contactPoint = new Vector2(_circleCenter.x - float_1 * float_5 / float_4, _circleCenter.y - float_2 * float_5 / float_4);
			_intersectionPoint1 = contactPoint;
			_intersectionPoint2 = contactPoint;
			return true;
		}
		else
		{
			return false;
		}
	}

	/// <summary>
	/// 円と円の交点の座標を求める
	/// </summary>
	/// <returns><c>true</c>円と円が交わっている時<c>false</c>円と円が交わらない時</returns>
	/// <param name="_circlePoint1">円1の座標</param>
	/// <param name="_circleRadius1">円1の半径</param>
	/// <param name="_circlePoint2">円2の座標</param>
	/// <param name="_circleRadius2">円2の半径</param>
	/// <param name="_intersectionPoint1">交点1</param>
	/// <param name="_intersectionPoint2">交点2</param>
	public static bool GetIntersectionOfCircleAndCircle(Vector2 _circlePoint1, float _circleRadius1, Vector2 _circlePoint2, float _circleRadius2, out Vector2 _intersectionPoint1, out Vector2 _intersectionPoint2)
	{
		_intersectionPoint1 = Vector2.zero;
		_intersectionPoint2 = Vector2.zero;
		var a = _circlePoint1.x - _circlePoint2.x;
		var b = _circlePoint1.y - _circlePoint2.y;
		var c = 0.5f * ((_circleRadius1 - _circleRadius2) * (_circleRadius1 + _circleRadius2) - a * (_circlePoint1.x + _circlePoint2.x) - b * (_circlePoint1.y + _circlePoint2.y));
		var l = a * a + b * b;
		var k = a * _circlePoint1.x + b * _circlePoint1.y + c;
		var d = l * _circleRadius1 * _circleRadius1 - k * k;
		if (d > 0)
		{
			var ds = Mathf.Sqrt(d);
			var apl = a / l;
			var bpl = b / l;
			var xc = _circlePoint1.x - apl * k;
			var yc = _circlePoint1.y - bpl * k;
			var xd = bpl * ds;
			var yd = apl * ds;
			_intersectionPoint1 = new Vector2(xc - xd, yc + yd);
			_intersectionPoint2 = new Vector2(xc + xd, yc - yd);
			return true;
		}
		else if (d.Equals(0.0f))
		{
			_intersectionPoint1 = new Vector2(_circlePoint1.x - a * k / l, _circlePoint1.y - b * k / l);
			_intersectionPoint2 = new Vector2(_circlePoint1.x - a * k / l, _circlePoint1.y - b * k / l);
			return true;
		}
		else
		{
			return false;
		}
	}

	#endregion

	#region 角度とベクトルなど

	#region VectorAndAngle(ベクトルと角度関係)

	/// <summary>
	/// ベクトルから角度に直す
	/// </summary>
	/// <param name="_vec">ベクトル</param>
	/// <returns>角度</returns>
	public static float VectorToAngle(Vector2 _vec)
	{
		var angle = (Mathf.Atan2(_vec.normalized.y, _vec.normalized.x) * Mathf.Rad2Deg) + 270.0f;
		if (angle < 0) { angle += 360.0f; } else if (angle > 360) { angle -= 360.0f; }
		return angle;
	}

	#endregion

	#endregion

	#region 配列など

	#region ArraySum(配列内の要素を合計する)

	/// <summary>
	/// 配列内の要素を合計する
	/// </summary>
	/// <param name="_array">合計する配列</param>
	/// <returns>合計値</returns>
	public static int ArraySum(params int[] _array)
	{
		int sum = 0;
		for (int i = 0; i < _array.Length; i++)
			sum += _array[i];
		return sum;
	}

	/// <summary>
	/// 配列内の要素を合計する
	/// </summary>
	/// <param name="_array">合計する配列</param>
	/// <returns>合計値</returns>
	public static float ArraySum(params float[] _array)
	{
		float sum = 0;
		for (int i = 0; i < _array.Length; i++)
			sum += _array[i];
		return sum;
	}
	#endregion

	#region ArrayMax(配列内の要素で最大の値を返す)

	/// <summary>
	/// 配列内の最大値を返す
	/// </summary>
	/// <param name="_array">配列</param>
	/// <returns>最大値</returns>
	public static int ArrayMax(params int[] _array)
	{
		int max = _array[0];
		for (int i = 0; i < _array.Length; i++)
		{
			if (_array[i] > max)
				max = _array[i];
		}
		return max;
	}

	/// <summary>
	/// 配列内の最大値を返す
	/// </summary>
	/// <param name="_array">配列</param>
	/// <returns>最大値</returns>
	public static float ArrayMax(params float[] _array)
	{
		float max = _array[0];
		for (int i = 0; i < _array.Length; i++)
		{
			if (_array[i] > max)
				max = _array[i];
		}
		return max;
	}

	#endregion

	#region ArrayMin(配列内の要素で最小値を返す)

	/// <summary>
	/// 配列内の最小値を返す
	/// </summary>
	/// <returns>最小値</returns>
	/// <param name="_array">配列</param>
	public static int ArrayMin(params int[] _array)
	{
		int min = _array[0];
		for (int i = 0; i < _array.Length; i++)
		{
			if (_array[i] < min)
				min = _array[i];
		}
		return min;
	}

	/// <summary>
	/// 配列内の最小値を返す
	/// </summary>
	/// <returns>最小値</returns>
	/// <param name="_array">配列</param>
	public static float ArrayMin(params float[] _array)
	{
		float min = _array[0];
		for (int i = 0; i < _array.Length; i++)
		{
			if (_array[i] < min)
				min = _array[i];
		}
		return min;
	}

	#endregion

	#region ArrayMaxIndex(配列内の要素で最大値の添字を返す)
	/// <summary>
	/// 配列内の最大値の添字を返す
	/// </summary>
	/// <returns>最大値の添字</returns>
	/// <param name="_array">配列</param>
	public static int ArrayMaxIndex(params int[] _array)
	{
		int maxIdx = 0;
		for (int i = 0; i < _array.Length; i++)
		{
			if (_array[i] > _array[maxIdx])
				maxIdx = i;
		}
		return maxIdx;
	}

	/// <summary>
	/// 配列内の最大値の添字を返す
	/// </summary>
	/// <returns>最大値の添字</returns>
	/// <param name="_array">配列</param>
	public static float ArrayMaxIndex(params float[] _array)
	{
		int maxIdx = 0;
		for (int i = 0; i < _array.Length; i++)
		{
			if (_array[i] > _array[maxIdx])
				maxIdx = i;
		}
		return maxIdx;
	}
	#endregion

	#region ArrayMinIndex(配列内の要素で最小値の添字を返す)
	/// <summary>
	/// 配列内の最小値の添字を返す
	/// </summary>
	/// <returns>最小値の添字</returns>
	/// <param name="_array">配列</param>
	public static int ArrayMinIndex(params int[] _array)
	{
		int minIdx = 0;
		for (int i = 0; i < _array.Length; i++)
		{
			if (_array[i] < _array[minIdx])
				minIdx = i;
		}
		return minIdx;
	}

	/// <summary>
	/// 配列内の最小値の添字を返す
	/// </summary>
	/// <returns>最小値の添字</returns>
	/// <param name="_array">配列</param>
	public static float ArrayMinIndex(params float[] _array)
	{
		int minIdx = 0;
		for (int i = 0; i < _array.Length; i++)
		{
			if (_array[i] < _array[minIdx])
				minIdx = i;
		}
		return minIdx;
	}
	#endregion

	#endregion

	#region ランダムなど

	#region GetRandomIndex(重みづけされた配列からランダムな添え字を返す)

	/// <summary>
	/// 重みづけされた配列からランダムな添え字を返す
	/// 配列の要素が全て0の時は-1を返す
	/// </summary>
	/// <param name="_weightTable">重み付けされた配列</param>
	/// <returns>ランダムな添え字</returns>
	public static int GetRandomIndex(params int[] _weightTable)
	{
		float[] floatWeighttable = new float[_weightTable.Length];
		_weightTable.CopyTo(floatWeighttable, 0);
		return GetRandomIndex(floatWeighttable);
	}

	/// <summary>
	/// 重みづけされた配列からランダムな添え字を返す
	/// 配列の要素が全て0の時は-1を返す
	/// </summary>
	/// <param name="_weightTable">重み付けされた配列</param>
	/// <returns>ランダムな添え字</returns>
	public static int GetRandomIndex(params float[] _weightTable)
	{
		float totalWeight = ArraySum(_weightTable);
		float val = UnityEngine.Random.Range(0.0f, totalWeight);
		int retIndex = -1;
		for (int i = 0; i < _weightTable.Length; i++)
		{
			if (_weightTable[i] >= val)
			{
				retIndex = i;
				break;
			}
			val -= _weightTable[i];
		}
		return retIndex;
	}

	#endregion

	#region RandomDateTime(開始時間と終了時間をランダムで返す)

	/// <summary>
	///	開始時間と終了時間の間をランダムで返す計算をする
	/// </summary>
	/// <param name="_startDateTime">開始時間</param>
	/// <param name="_endDatetime">終了時間</param>
	/// <returns>開始時間～終了時間の間のランダムな時間</returns>
	public static DateTime RandomDateTime(DateTime _startDateTime, DateTime _endDatetime)
	{
		if (_startDateTime > _endDatetime)
			return _startDateTime;

		TimeSpan span = _endDatetime - _startDateTime;
		float ranSpan = UnityEngine.Random.Range(0, (float)span.TotalMilliseconds);
		return _startDateTime + TimeSpan.FromMilliseconds(ranSpan);
	}
	#endregion

	#endregion

	#region その他

	/// <summary>
	/// 式
	/// </summary>
	public class Function
	{
		public float a = 0;
		public float b = 0;

		public Function() { }
		public Function(float _a, float _b)
		{
			a = _a;
			b = _b;
		}
	}

	/// <summary>
	/// 座標
	/// </summary>
	public class Coordinates
	{
		public float x = 0;
		public float y = 0;

		public Coordinates() { }
		public Coordinates(float _x, float _y)
		{
			x = _x;
			y = _y;
		}
	}

	/// <summary>
	/// 二点の座標から式を求める
	/// </summary>
	/// <returns>The quadratic function.</returns>
	/// <param name="_coordinates1">座標1</param>
	/// <param name="_coordinates2">座標2</param>
	public static Function GetQuadraticFunction(Coordinates _coordinates1, Coordinates _coordinates2)
	{
		var func = new Function();
		func.a = (_coordinates2.y - _coordinates1.y) / (_coordinates2.x - _coordinates1.x);
		func.b = _coordinates1.y - _coordinates1.x;
		return func;
	}

	/// <summary>
	/// 二つの式の交点の座標を求める
	/// </summary>
	/// <returns>The cross coordinates.</returns>
	/// <param name="_func1">式1</param>
	/// <param name="_func2">式2</param>
	public static Coordinates GetCrossCoordinates(Function _func1, Function _func2)
	{
		var coordinates = new Coordinates(
			(_func2.b - _func1.b) / (_func1.a - _func2.a),
			((_func1.a * _func2.b) - (_func2.a * _func1.b)) / (_func1.a - _func2.a));
		return coordinates;
	}

	/// <summary>
	/// 四点から交点の座標を求める
	/// </summary>
	/// <returns>The cross coordinates.</returns>
	/// <param name="_point1_1">Point1 1.</param>
	/// <param name="_point1_2">Point1 2.</param>
	/// <param name="_point2_1">Point2 1.</param>
	/// <param name="_point2_2">Point2 2.</param>
	public static Coordinates GetCrossCoordinates(Coordinates _point1_1, Coordinates _point1_2, Coordinates _point2_1, Coordinates _point2_2)
	{
		var a0 = (_point1_2.y - _point1_1.y) / (_point1_2.x - _point1_1.x);
		var a1 = (_point2_2.y - _point2_1.y) / (_point2_2.x - _point2_1.x);
		var x = (a0 * _point1_1.x - _point1_1.y - a1 * _point2_1.x + _point2_1.y) / (a0 - a1);
		var y = (_point1_2.y - _point1_1.y) / (_point1_2.x - _point1_1.x) * (x - _point1_1.x) + _point1_1.y;
		return new Coordinates(x, y);
	}

	#endregion
}