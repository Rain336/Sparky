# To-Do List

## Needs Fixes

- [ ] Elements need testing. See below.
- [ ] Actor Manager is not implemented.
- [x] Select appropriate Renderer on Android. Currently is uses fixed Vulken.
- [ ] Test Rendering! Very Important! Both Renderers!

## Get it Running

- [x] Implement Stateful Component
- [x] Implement Actor Component
- [x] Implement Startup Code for Rendering on Android
- [ ] Implement Startup Code for Rendering on iOS
- [x] Implement IDrawable
- [ ] Implement IInteractable
- [ ] Implement Child Layouting (Most likely ILayoutChildren)
- [ ] ActorWithChildComponent for simplicity
- [ ] ActorWithChildrenComponent for simplicity

## Unit Tests

- [x] BoxTests -> Check if parsing of Box works correctly.
- [x] UnitTests -> Check if parsing of Unit works correctly.
- [x] StatelessComponentTests -> Check if Component Render method is called and Element tree is correct.
- [x] StatefulComponentTests -> Same as Stateless plus check again with new values after ReRender after SetState.
- [x] StatefulComponentOfTests -> Same as Stateful tests.
- [ ] RootComponentTests -> Test if child is rendered and mounted and Actor has correct width and height.
- [x] ActorComponentTests -> Check if Actor are correctly built.
- [x] ActorComponentWithChildTests -> Check if Actor and Element tree are correctly built.
- [x] ActorComponentWithChildrenTests -> Check if Actor and Element tree are correctly built.

## Ideas

- [ ] Component testing library for end users, similar to react.
- [ ] Updates and Keying instead of Rebuilding every time
- [ ] Some form auf inherited Context you can depend on! Like InheritedWidget from Flutter

## Not possible right now

- [ ] When SkiaSharp updates to Skia 89 or higher, use GrBackedSemaphore in Vulkan Renderer