using System.Collections.Generic;

public static class PlayerProgress
{
    private static readonly HashSet<string> clearedChapterIds =
        new HashSet<string>();

    public static bool IsChapterCleared(ChapterData chapter)
    {
        return chapter != null &&
               !string.IsNullOrEmpty(chapter.ChapterId) &&
               clearedChapterIds.Contains(chapter.ChapterId);
    }

    public static void MarkChapterCleared(ChapterData chapter)
    {
        if (chapter == null || string.IsNullOrEmpty(chapter.ChapterId))
        {
            return;
        }

        clearedChapterIds.Add(chapter.ChapterId);
    }
}