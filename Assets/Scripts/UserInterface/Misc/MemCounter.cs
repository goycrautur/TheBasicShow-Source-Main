using UnityEngine;
using TMPro;
using Unity.Profiling;

public class MemCounter : MonoBehaviour
{
    public ProfilerRecorder totalReservedMemoryRecorder;
    public float memmain,mem;
    [SerializeField] private TMP_Text memText;
    private void OnEnable()
    {
        totalReservedMemoryRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Total Reserved Memory");
    }

    private void OnDisable()
    {
        totalReservedMemoryRecorder.Dispose();
    }
    private void Update()
    {
        mem = totalReservedMemoryRecorder.LastValue;
        if (mem > 1000)
        {
            memText.text = "Memory: " + mem / 1000 + " KB";
        }
        if (mem > 1000000)
        {
            memText.text = "Memory: " + mem / 1000000 + " MB";
        }
        if (mem > 1000000000)
        {
            memText.text = "Memory: " + mem/1000000000 + " GB";
        }
    }
}