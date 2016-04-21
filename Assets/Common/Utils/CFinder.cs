/**
	通用查找。按组件查找、按名字查找(主要用于教学) CommonFinder的缩写
	
	eg.按照名字查找指定名字的控件，用于教学
	CFinder.Find(transform, result, new GOFinderByIteration(new GOFinderNameIter("EquipIcon")));
	
	Added by Teng.
**/
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class CFinder
{
	/**
		查找指定根节点下符合指定的查找器的Transform并保持到findResult中
		
		param:Transform root 查找根节点
		param:List<Transform> find_result 查找到的列表
		param:IGOFinder finder 查找过滤器
	**/
    public static void Find(Transform root, List<Transform> find_result, IGOFinder finder)
    {
        if (root == null)
        {
            throw new Exception("root can not be null, it defines the starting point of the find path!");
        }

        if (find_result == null)
        {
            throw new Exception("findResult can not be null, it used to collect the find result!");
        }

        if (finder == null)
        {
            throw new Exception("finder can not be null, it defines how to find transform");
        }
        finder.Find(root, find_result);
    }
	
	public static void FindGameObjectInChildren(Transform root, List<Transform> find_result, string control_name)
	{
		Find(root, find_result, new GOFinderByIteration(new GOFinderNameIter(control_name)));
	}
}

// 游戏对象搜索接口 Interface + Game + Object + Finder
public interface IGOFinder
{
	// 搜索
	// param: Transform root 搜索的开始位置/根节点
	// param: List<Transform> find_result 搜索存放的结果
	void Find(Transform root, List<Transform> find_result);
}

/**
	根据组件搜索
	功能相当于:GetComponentsInChildren
	
	eg. CFinder.Find(transform, result, new GOFinderByComponent<Transform>());
**/
public class GOFinderByComponent<T> : IGOFinder where T : Component
{
	// 实现查找接口
	public void Find(Transform root, List<Transform> find_result)
	{
		foreach (var componentsInChild in root.GetComponentsInChildren<T>())
		{
			find_result.Add(componentsInChild.transform);
		}
	}
}

/**
	迭代遍历搜索
	
	eg.按照名字查找指定名字的控件，用于教学
	CFinder.Find(transform, result, new GOFinderByIteration(new GOFinderNameIter("EquipIcon")));
**/
public class GOFinderByIteration : IGOFinder
{
	private IGOFinderIter finderForIteration;
	public GOFinderByIteration(IGOFinderIter finderForIteration)
	{
		this.finderForIteration = finderForIteration;
	}
	
	// 实现接口
	public void Find(Transform root, List<Transform> findResult)
	{
		Transform t;
		for (int i = 0, childCount = root.childCount; i < childCount; i++)
		{
			t = root.GetChild(i);
			if (finderForIteration.isVaild(t))
			{
				findResult.Add(t);
			}
			Find(t, findResult);
		}
	}
}

// 迭代搜索判断
public interface IGOFinderIter
{
	// 指定节点是否合法
	bool isVaild(Transform node);
}

// 迭代遍历按名字搜索
public class GOFinderNameIter : IGOFinderIter
{
	protected readonly string name;
	
	public GOFinderNameIter(string _name)
	{
		name = _name;
	}
	
	// 接口
	public bool isVaild(Transform child)
	{
		return child.gameObject.name.Equals(name);
	}
}





