using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace VTLTools
{
    public static class AnimationUtils
    {
        public static Sequence PlayHandTUT(Transform pivot, Vector3 startPos, Vector3 endPos, Image img = null, SpriteRenderer sprite = null, bool isLoop = true, Vector3 originScale = default)
        {
            if (originScale == default)
                originScale = Vector3.one;
            Sequence _seq = DOTween.Sequence();
            if (img != null)
            {
                img.gameObject.SetActive(true);
                img.transform.localScale = originScale * 1.2f;
                img.transform.position = startPos;
                var color = img.color;
                color.a = 1f;
                img.color = color;
                _seq.Append(img.transform.DOScale(originScale, 0.5f).SetEase(Ease.InOutSine));
                _seq.Append(img.transform.DOMove(endPos, 0.5f).SetEase(Ease.InOutSine));
                _seq.Append(img.transform.DOScale(originScale * 1.3f, 0.5f).SetEase(Ease.InOutSine));
                _seq.Append(img.DOFade(0f, 0.5f));
            }
            else if (sprite != null)
            {
                pivot.gameObject.SetActive(true);
                pivot.transform.localScale = Vector3.one * 1.2f;
                pivot.transform.position = startPos;
                var color = sprite.color;
                color.a = 1f;
                sprite.color = color;
                _seq.Append(pivot.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.InOutSine));
                _seq.Append(pivot.transform.DOMove(endPos, 0.5f).SetEase(Ease.InOutSine));
                _seq.Append(pivot.transform.DOScale(Vector3.one * 1.3f, 0.5f).SetEase(Ease.InOutSine));
                _seq.Append(sprite.DOFade(0f, 0.5f));
            }
            if (isLoop)
            {
                _seq.AppendInterval(0.5f); // Thêm delay trước khi lặp lại
                _seq.SetLoops(-1, LoopType.Restart); // Lặp vô hạn
            }
            return _seq;
        }

        public static Sequence PlayNotifyWinLevel(Image img)
        {
            Sequence _seq = DOTween.Sequence();
            if (img != null)
            {
                img.gameObject.SetActive(true);
                img.transform.localScale = Vector3.one * 0.5f;
                var color = img.color;

                img.color = color;
                _seq.Append(img.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack));
                _seq.Join(img.DOFade(1f, 0.5f));
                _seq.AppendInterval(1f);
                _seq.Append(img.transform.DOScale(Vector3.one * 1.2f, 0.5f).SetEase(Ease.InBack));
                _seq.Join(img.DOFade(0f, 0.5f));
                _seq.OnComplete(() => img.gameObject.SetActive(false));
            }

            return _seq;
        }

        public static Sequence PlayAnimShow(Transform target, float startScale = 0, float endScale = 1)
        {
            Sequence _seq = DOTween.Sequence();
            target.gameObject.SetActive(true);
            target.localScale = Vector3.one * startScale;
            _seq.Append(target.DOScale(Vector3.one * endScale, 0.5f).SetEase(Ease.OutBack));
            _seq.AppendInterval(1f);
            _seq.OnComplete(() => target.gameObject.SetActive(false));
            return _seq;
        }

        public static Sequence AnimScoreWorldSpace(Transform pivot, Text text, Vector3 startPos, Vector3 endPos, System.Action onComplete = null, bool moreRotate = false, float speedAvg = 0.5f)
        {
            Sequence _seq = DOTween.Sequence();
            text.color = text.color.SetAlpha(0f);
            pivot.gameObject.SetActive(true);
            pivot.transform.position = startPos;
            _seq.Append(pivot.DOMove(endPos, speedAvg).SetEase(Ease.InOutSine));
            _seq.Join(text.DOFade(1f, speedAvg));
            // Lấy rotation hiện tại theo Euler
            Vector3 currentEuler = pivot.transform.eulerAngles;
            // Tạo góc xoay Z ngẫu nhiên
            if (moreRotate)
            {
                float randomZ = Random.Range(-30f, 30f);
                Vector3 targetEuler = new Vector3(currentEuler.x, currentEuler.y, randomZ);
                //DPDebug.Log($"<color=green>[DA]</color> {pivot.transform.eulerAngles} - {targetEuler}");
                _seq.Append(pivot.DORotate(targetEuler, speedAvg, RotateMode.Fast));
            }
            _seq.AppendInterval(0.3f);
            _seq.OnComplete(() =>
                {
                    onComplete?.Invoke();
                });
            return _seq;
        }

        /// <summary>
        /// Rotate transform around a world-space center (2D – Z axis)
        /// </summary>
        public static Tween RotateAround2D(
            this Transform target,
            Vector3 worldCenter,
            float angle,
            float duration,
            RotateMode rotateMode = RotateMode.FastBeyond360,
            Ease ease = Ease.Linear
        )
        {
            Vector3 offset = target.position - worldCenter;

            float currentAngle = 0f;

            return DOTween.To(
                () => currentAngle,
                a =>
                {
                    float delta = a - currentAngle;
                    currentAngle = a;

                    // xoay offset quanh trục Z
                    offset = Quaternion.AngleAxis(delta, Vector3.forward) * offset;
                    target.position = worldCenter + offset;
                },
                angle,
                duration
            )
            .SetEase(ease)
            .SetTarget(target);
        }

        /// <summary>
        /// Rotate transform around an arbitrary pivot point in world space (2D – Z axis),
        /// without changing real transform pivot.
        /// Can be used to switch pivot between animation phases.
        /// </summary>
        public static Tween RotateWithVirtualPivot2D(
            this Transform target,
            Vector3 worldPivot,
            float angle,
            float duration,
            Ease ease = Ease.Linear,
            RotateMode rotateMode = RotateMode.FastBeyond360
        )
        {
            Quaternion startRot = target.rotation;
            Vector3 startPos = target.position;

            float currentAngle = 0f;

            return DOTween.To(
                () => currentAngle,
                a =>
                {
                    float delta = a - currentAngle;
                    currentAngle = a;

                    // xoay rotation thật
                    target.rotation *= Quaternion.AngleAxis(delta, Vector3.forward);

                    // xoay position quanh pivot ảo
                    Vector3 dir = target.position - worldPivot;
                    dir = Quaternion.AngleAxis(delta, Vector3.forward) * dir;
                    target.position = worldPivot + dir;
                },
                angle,
                duration
            )
            .SetEase(ease)
            .SetTarget(target);
        }
    }
}