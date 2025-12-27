using YanickSenn.Utils.Control;
using YanickSenn.Utils.Events;

namespace YanickSenn.Utils.Extensions {
    public static class OptionalExtensions {
        public static void InvokeIfPresent(this Optional<GlobalEvent> globalEvent) {
            globalEvent.DoIfPresent(e => e.Invoke());
        }
        public static void InvokeIfPresent(this Optional<GlobalEvent> globalEvent, GlobalEvent.Sender sender) {
            globalEvent.DoIfPresent(e => e.Invoke(sender));
        }
        public static void InvokeIfPresent(this Optional<GlobalEvent> globalEvent, object payload) {
            globalEvent.DoIfPresent(e => e.Invoke(payload));
        }
        public static void InvokeIfPresent(this Optional<GlobalEvent> globalEvent, object payload, GlobalEvent.Sender sender) {
            globalEvent.DoIfPresent(e => e.Invoke(payload, sender));
        }
    }
}
