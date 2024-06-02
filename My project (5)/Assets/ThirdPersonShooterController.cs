using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using StarterAssets;
using UnityEngine.InputSystem;
using UnityEngine.Animations.Rigging;

public class ThirdPersonShooterController : MonoBehaviour
{
   [SerializeField] private CinemachineVirtualCamera aimVirtualCamera;
   [SerializeField] private float normalSensitivity;
   [SerializeField] private float aimSensitivity;
   [SerializeField] private LayerMask aimColliderLayerMask = new LayerMask();
   [SerializeField] private Transform debugTransform;
   [SerializeField] private Transform pfBulletProjectile;
   [SerializeField] private Transform spawnBulletPosition;
   //VFX
   [SerializeField] private Transform vfxGreen;
   [SerializeField] private Transform vfxRed;
   [SerializeField] private ParticleSystem gunLight;
   //IK
   [SerializeField] private Rig aimRig;

   private ThirdPersonController thirdPersonController;
   private StarterAssetsInputs _input;
   private Animator anim;
   private float animRightWeight;
   private Camera _cam;

   private void Awake() 
   {
      thirdPersonController = GetComponent<ThirdPersonController>();
      _input = GetComponent<StarterAssetsInputs>();
      anim = GetComponent<Animator>();
      _cam = Camera.main;
   }

   private void Update() 
   {
      Vector3 mouseWorldPosition = Vector3.zero;
      Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
      Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
      debugTransform.position = ray.GetPoint(20);//射線沒碰撞
      //起點位置+射線向量  打到的物件  射線長度  忽略的塗層
      Transform hitTransform = null;
      if(Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimColliderLayerMask))
      {
         debugTransform.position = raycastHit.point;
         mouseWorldPosition = raycastHit.point;
         hitTransform = raycastHit.transform;
         if(_input.shoot)
         {
            if(raycastHit.collider.gameObject.TryGetComponent<Rigidbody>(out Rigidbody rigidbody))
            {
               rigidbody.AddExplosionForce(1000f, debugTransform.position, 5f);
            }
         }
      }


      if(_input.aim)
      {
         aimVirtualCamera.gameObject.SetActive(true);
         thirdPersonController.SetSensitivity(aimSensitivity);

         Vector3 worldAimTarget = debugTransform.position;
         worldAimTarget.y = transform.position.y;
         //目標位置- 玩家位置   normalized 把座標
         Vector3 aimDirection = (worldAimTarget - transform.position).normalized;
         //用Lerp插植 絲滑的轉角色面向
         transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 20f);
         //移動時不會和瞄準搶旋轉
         thirdPersonController.SetRotateOnMove(false);
         anim.SetLayerWeight(1, Mathf.Lerp(anim.GetLayerWeight(1), 1f, Time.deltaTime * 10f));
         animRightWeight = 1f;
      }
      else
      {
         aimVirtualCamera.gameObject.SetActive(false);
         thirdPersonController.SetSensitivity(normalSensitivity);
         thirdPersonController.SetRotateOnMove(true);
         anim.SetLayerWeight(1, Mathf.Lerp(anim.GetLayerWeight(1), 0f, Time.deltaTime * 10f));
         animRightWeight = 0f;
      }
      
      if(_input.shoot)
      {
         //if(hitTransform != null)
         //{
            //if(hitTransform.GetComponent<BulletTarget>() != null)
            //{
               //Instantiate(vfxGreen, debugTransform.position, Quaternion.identity);
            //}
            //else
            //{
               //Instantiate(vfxRed, debugTransform.position, Quaternion.identity);
            //}
         //}
         
         Vector3 aimDir = (mouseWorldPosition - spawnBulletPosition.position).normalized;
         Instantiate(pfBulletProjectile, spawnBulletPosition.position, Quaternion.LookRotation(aimDir, Vector3.up));
         GameObject.Find("pfBulletProjectile(Clone)").GetComponent<BulletProjectile>().Setup(debugTransform.position);

         _input.shoot = false;
         gunLight.Play();
         
      }
      aimRig.weight = Mathf.Lerp(aimRig.weight, animRightWeight, Time.deltaTime * 20f);
      //Debug.Log(debugTransform.position);
   }
}
