using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonHoverHandler : MonoBehaviour
{
	public bool OnHover = false;
	//   private char type = 'A';
	//   public SetTowerBaseInput dinamicPanel;
	//   public Text descriptionBox;

	//   public Tower towerReference;

	//   public RawImage imageA;
	//   public RawImage imageB;
	//   public Sprite[] towerSpot;
	//   public Texture cameraTower;

	//   void Start()
	//   {
	//	towerReference = dinamicPanel.transform.GetChild(2).GetComponent<Tower>();
	//}

	//   private void Update()
	//   {
	//       if (dinamicPanel.spawnTower)
	//       {
	//           if (type == 'A')
	//           {
	//               imageA.texture = cameraTower;
	//           }
	//		if (type == 'B')
	//		{
	//			imageB.texture = cameraTower;
	//		}
	//	}
	//       else 
	//       {
	//           //if (!OnHover)
	//           //{
	//           //    hoverImage.GetComponent<Animator>().SetBool("Open", false);
	//           //}
	//           //else 
	//           //{ 
	//           //    hoverImage.GetComponent<Animator>().SetBool("Open", true);
	//           //}
	//           //hoverImage.transform.GetChild(1).GetComponent<Image>().enabled = false;

	//           switch (dinamicPanel.towerGrup)
	//           {

	//               case 0:
	//                   imageA.texture = towerSpot[0].texture;
	//                   imageB.texture = towerSpot[1].texture;
	//				break;
	//               case 1:
	//                   imageA.texture = towerSpot[2].texture;
	//                   imageB.texture = towerSpot[3].texture;
	//                   break;
	//               case 2:
	//                   imageA.texture = towerSpot[4].texture;
	//                   imageB.texture = towerSpot[5].texture;
	//                   break;
	//           }
	//       }
	//   }
	public void OnHovering(bool OnHover)
	{
		this.OnHover = OnHover;
	}
	//   public void TypeA()
	//   {
	//       type = 'A';
	//   }

	//   public void TypeB()
	//   {
	//       type = 'B';
	//   }
}