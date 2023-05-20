using System;
using Basilisk.Autoloads;
using Chickensoft.GoDotLog;
using Chickensoft.GoDotNet;
using Godot;
using MonoCustomResourceRegistry;

namespace Basilisk.Ui;

[RegisteredType(nameof(InsanityVignette), baseType: nameof(ColorRect))]
public partial class InsanityVignette: ColorRect
{
    private ShaderMaterial ShaderMaterial => Material as ShaderMaterial ?? throw new InvalidCastException();
    private InsanityManager InsanityManager => this.Autoload<InsanityManager>();
    [Export] private float _hitIntensity = 1f;
    private ILog _log = new GDLog(nameof(InsanityVignette));
    [Export] private float _animDuration = 0.5f;
    private float Offset => 0.4f;

    public override void _Ready()
    {
        InsanityManager.InsanityChanged += OnInsanityChanged;
    }
    
    private void SetShaderBlur(float blur)
    {
        ShaderMaterial.SetShaderParameter("blur", blur);
    }
    
    private void SetShaderIntensity(float intensity)
    {
        _log.Print($"Setting insanity to {intensity}");
        ShaderMaterial.SetShaderParameter("intensity", intensity);
    }
    
    private float GetShaderIntensity()
    {
        return (float)ShaderMaterial.GetShaderParameter("intensity");
    }

    private void OnInsanityChanged(int level, int percent)
    {
        float newValue = percent / 100.0f;
        float oldValue = GetShaderIntensity();
        var hitTween = GetTree().CreateTween();
        hitTween.SetTrans(Tween.TransitionType.Bounce);

        if(newValue < _hitIntensity)
        {
            hitTween.TweenMethod(Callable.From<float>(SetShaderIntensity), oldValue,  _hitIntensity, _animDuration);
            hitTween.TweenMethod(Callable.From<float>(SetShaderIntensity), _hitIntensity, newValue + Offset, _animDuration);
            hitTween.Play();
            return;
            
        }
        
        hitTween.TweenMethod(Callable.From<float>(SetShaderIntensity), _hitIntensity, newValue + Offset, _animDuration);
        hitTween.Play();
    }
}