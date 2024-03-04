using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Coffee.UIExtensions;
using DG.Tweening;

public class DamageAnim : MonoBehaviour
{
	[SerializeField] private UIEffect effectImage;
	[SerializeField] private Color color_Hit, color_Normal;
	[SerializeField] private float strengthShake;

	Tween anim_Color;
	Tween animShake;

	Vector3 originPos;

	[ContextMenu("Anim")]
	public void TriggerAnim()
	{
		if (anim_Color != null)
			anim_Color.Kill();

		if (animShake != null)
			animShake.Kill();

		transform.localPosition = originPos;

		animShake = transform.DOShakePosition(0.7f,strengthShake);
		effectImage.effectColor = color_Normal;
		anim_Color = DOTween.To(() => effectImage.effectColor, x => effectImage.effectColor = x, color_Hit, 0.5f).SetEase(Ease.Linear).SetLoops(2, LoopType.Yoyo);

	}

	private void OnEnable()
	{
		originPos += this.transform.localPosition;
	}
}
