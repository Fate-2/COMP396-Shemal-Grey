using UnityEngine;
using UnityEngine.Audio;
using TMPro;
using UnityEngine.UI;
public class VolumeManager : MonoBehaviour
{
    // For variables
    // We need references for VolumeSlider prefab, VolumeScrolViewContent transform, AudioMixer
    [SerializeField] private GameObject volumeSliderPrefab;
    [SerializeField] private Transform volumeScrollViewContent;
    [SerializeField] private AudioMixer volumeMixer;
    private void Start()
    {
        // Get all the groups from AudioMixer
        // Loop on the group and use them on the Prefab
        // Instantiate the prefab
        // Set the parent of the prefab as the Content in ScrollView
        // Get the TextMeshPro and change to match the group name
        // Get the Slider and add the method for changing the volume from that group
        var groups = volumeMixer.FindMatchingGroups("Master");
        foreach (var group in groups)
        {
            var volumeGameObject = Instantiate(volumeSliderPrefab, volumeScrollViewContent);
            volumeGameObject.GetComponentInChildren<TMP_Text>().text = group.ToString();
            volumeGameObject.GetComponentInChildren<Slider>().onValueChanged.AddListener(value =>
            {
                //volumeMixer.SetFloat(group.name+"Volume", Mathf.Log10(value)*20);
                volumeMixer.SetFloat(group.name + "Volume", value); // Using 20db and -80db
            });
        }
    }
}